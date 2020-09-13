using PacLang.CodeAnalysis.Syntax;
using System;
using System.Collections.Generic;

namespace PacLang.Binding
{
  internal sealed class Binder
  {
    private readonly DiagnosticBag _diagnostics = new DiagnosticBag();

    public DiagnosticBag Diagnostics => _diagnostics;

    public BoundExpression BindExpression(ExpressionSyntax syntax)
    {
      return syntax.Kind switch
      {
        SyntaxKind.ParenthesizeExpression => BindParenthesizedExpression(((ParenthesizedExpressionSyntax)syntax)),
        SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
        SyntaxKind.NameExpression => BindNameExpression((NameExpresionSyntax)syntax),
        SyntaxKind.AssigmentExpression => BindAssignmentExpression((AssignmentExpresionSyntax)syntax),
        SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
        SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
        _ => throw new Exception($"Unexpected syntax {syntax.Kind}"),
      };
    }

    private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax expression)
    {
      return BindExpression(expression.Expression);
    }

    private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
    {

      var value = syntax.Value ?? 0;
      return new BoundLiteralExpression(value);
    }

    private BoundExpression BindNameExpression(NameExpresionSyntax syntax)
    {
      throw new NotImplementedException();
    }


    private BoundExpression BindAssignmentExpression(AssignmentExpresionSyntax syntax)
    {
      throw new NotImplementedException();
    }


    

   
    private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
    {
      var boundOperand = BindExpression(syntax.Operand);
      var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);

      if (boundOperator == null)
      {
        _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundOperand.Type);
        return boundOperand;
      }

      return new BoundUnaryExpression(boundOperator, boundOperand);
    }

    private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
    {
      var boundLeft = BindExpression(syntax.Left);
      var boundRight = BindExpression(syntax.Right);
      var boundOperatorKind = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

      if (boundOperatorKind == null)
      {
        _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.Span, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
        return boundLeft;
      }

      return new BoundBinaryExpression(boundLeft, boundOperatorKind, boundRight);
    }
  }

}