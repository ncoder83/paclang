using System.Collections.Generic;

namespace PacLang.CodeAnalysis.Syntax
{
    //
    // +1 
    // -1 * -3
    // -(3 + 3)
    //
    //
    //
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;

        private List<string> _diagnostics = new List<string>();

        private int _position;
        public Parser(string text)
        {
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);
            SyntaxToken token;

            do
            {
                token = lexer.NextToken();

                if (token.Kind != SyntaxKind.WhiteSpaceToken && token.Kind != SyntaxKind.BadToken)
                    tokens.Add(token);

            } while (token.Kind != SyntaxKind.EndOfFileToken);


            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }


        public IEnumerable<string> Diagnostics => _diagnostics;
        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;

            return (index >= _tokens.Length) ? _tokens[_tokens.Length - 1] : //_tokens[^1]
                                              _tokens[index];
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

            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expression = ParseExpression();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }


        private ExpressionSyntax ParseExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;

            
            var unirayOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            
            if(unirayOperatorPrecedence != 0 && unirayOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseExpression(unirayOperatorPrecedence);
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
                var right = ParseExpression(precedence);

                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

     

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:
                {
                    var left = NextToken();
                    var expression = ParseExpression();
                    var right = MatchToken(SyntaxKind.CloseParenthesisToken);
                    return new ParenthesizedExpressionSyntax(left, expression, right);
                }
                case SyntaxKind.TrueKeyword:
                case SyntaxKind.FalseKeyword:
                {
                    var keyworkdToken = NextToken();
                    var value = Current.Kind == SyntaxKind.TrueKeyword;
                    return new LiteralExpressionSyntax(keyworkdToken, value);
                }
                default: 
                {
                    var numberToken = MatchToken(SyntaxKind.NumberToken);
                    return new LiteralExpressionSyntax(numberToken);
                }
            }            
        }
    }
}
