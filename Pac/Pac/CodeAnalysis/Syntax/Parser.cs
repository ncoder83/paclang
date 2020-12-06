﻿using Pac.CodeAnalysis.Syntax;
using PacLang.Text;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PacLang.CodeAnalysis.Syntax
{
    //
    // +1 
    // -1 * -3
    // -(3 + 3)    
    internal sealed class Parser
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly ImmutableArray<SyntaxToken> _tokens;
        private readonly SourceText _text;

        private int _position;
        public Parser(SourceText text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);
            SyntaxToken token;

            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WhiteSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }

            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _text = text;
            _tokens = tokens.ToImmutableArray();
            _diagnostics.AddRange(lexer.Diagnostics);            
        }


        public DiagnosticBag Diagnostics => _diagnostics;
        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;

            if (index >= _tokens.Length)
                return _tokens[_tokens.Length - 1]; //_tokens[^1]
                                             
            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            _diagnostics.ReportUnexpectedToken(Current.Span, Current.Kind, kind);
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var statement = ParseStatement();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new CompilationUnitSyntax(statement, endOfFileToken);
        }

        private StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenBraceToken:
                    return ParseBlockStatement();
                case SyntaxKind.LetKeyword:
                case SyntaxKind.VarKeyword:
                    return ParseVariableDeclaration();
                case SyntaxKind.IfKeyword:
                    return ParseIfStatement();
                case SyntaxKind.WhileKeyword:
                    return ParseWhileStatement();
                case SyntaxKind.ForKeyword:
                    return ParseForStatement();
                default:
                    return ParseExpressionStatement();
            }            
        }

        private StatementSyntax ParseIfStatement()
        {
            var keyword = MatchToken(SyntaxKind.IfKeyword);
            var condition = ParseExpression();
            var statement = ParseStatement();
            var elseClause = ParseElseClause();
            return new IfStatementSyntax(keyword, condition, statement, elseClause);
        }

        private StatementSyntax ParseWhileStatement()
        {
            var keyword = MatchToken(SyntaxKind.WhileKeyword);
            var condition = ParseExpression();
            var body = ParseStatement();

            return new WhileStatementSyntax(keyword, condition, body);
        }

        private StatementSyntax ParseForStatement()
        {
            var keyword = MatchToken(SyntaxKind.ForKeyword);
            var identifier = MatchToken(SyntaxKind.IdentifierToken);
            var equalsToken = MatchToken(SyntaxKind.EqualsToken);

            var lowerBound = ParseExpression();
            var toKeyword = MatchToken(SyntaxKind.ToKeyword);
            var uppperBound = ParseExpression();
            var body = ParseStatement();

            return new ForStatementSyntax(keyword, identifier, equalsToken, lowerBound, toKeyword, uppperBound, body);
        }

        private ElseClauseSyntax ParseElseClause()
        {
            if (Current.Kind != SyntaxKind.ElseKeyword)
                return null;

            var keyword = NextToken();
            var statement = ParseStatement();
            return new ElseClauseSyntax(keyword, statement);
        }

        private StatementSyntax ParseVariableDeclaration()
        {
            var expected = Current.Kind == SyntaxKind.LetKeyword ? SyntaxKind.LetKeyword : SyntaxKind.VarKeyword;
            var keyword = MatchToken(expected);
            var identifier = MatchToken(SyntaxKind.IdentifierToken);
            var equals = MatchToken(SyntaxKind.EqualsToken);
            var initializer = ParseExpression();

            return new VariableDeclarationSyntax(keyword, identifier, equals, initializer);
        }

        private ExpressionStatementSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntax(expression);
        }

        private StatementSyntax ParseBlockStatement()
        {
            var statements = ImmutableArray.CreateBuilder<StatementSyntax>();

            var openBraceToken = MatchToken(SyntaxKind.OpenBraceToken);
            while(Current.Kind != SyntaxKind.EndOfFileToken && 
                  Current.Kind != SyntaxKind.CloseBraceToken) 
            {
                var startToken = Current;

                var statement = ParseStatement();
                statements.Add(statement);

                // if ParseStatement() did not consume any tokens,
                // we need to skip the current token and continue. We
                // do not need to report an error, because we will
                // already tired to parse an expression statement
                // and reporteed one
                if(Current == startToken) 
                {
                    NextToken();   
                }
            }

            var closeBraceToken = MatchToken(SyntaxKind.CloseBraceToken);
            return new BlockStatementSyntax(openBraceToken, statements.ToImmutable(), closeBraceToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseAssigmentExpression();
        }

        private ExpressionSyntax ParseAssigmentExpression()
        {

            if (Peek(0).Kind == SyntaxKind.IdentifierToken &&
              Peek(1).Kind == SyntaxKind.EqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssigmentExpression();
                return new AssignmentExpresionSyntax(identifierToken, operatorToken, right);
            }

            return ParseBinaryExpression();

        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;


            var unirayOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();

            if (unirayOperatorPrecedence != 0 && unirayOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unirayOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);

                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }



        private ExpressionSyntax ParsePrimaryExpression()
        {
            return Current.Kind switch
            {
                SyntaxKind.OpenParenthesisToken => ParseParenthesizedExpression(),
                SyntaxKind.TrueKeyword => ParseBooleanLiteral(),
                SyntaxKind.FalseKeyword => ParseBooleanLiteral(),
                SyntaxKind.NumberToken => ParseNumberLiteral(),
                SyntaxKind.IdentifierToken => ParseNameExpression(),
                _ => ParseNameExpression()
            };
        }

        private ExpressionSyntax ParseParenthesizedExpression()
        {
            var left = MatchToken(SyntaxKind.OpenParenthesisToken);
            var expression = ParseExpression();
            var right = MatchToken(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }
        private ExpressionSyntax ParseBooleanLiteral()
        {
            var isTrue = Current.Kind == SyntaxKind.TrueKeyword;
            var keyworkdToken = isTrue ? MatchToken(SyntaxKind.TrueKeyword): MatchToken(SyntaxKind.FalseKeyword);            
            return new LiteralExpressionSyntax(keyworkdToken, isTrue);
        }

        private ExpressionSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var identifierToken = MatchToken(SyntaxKind.IdentifierToken);
            return new NameExpresionSyntax(identifierToken);
        }
    }
}
