namespace MiniExpressionParser;

public class Interpreter : Expr.IVisitor<object>
{
  public object VisitBinaryExpression(Expr.Binary expr)
  {
    var left = Evaluate(expr.Left);
    var right = Evaluate(expr.Right);
    switch (expr.Op.TokenType)
    {
      case TokenType.Minus:
        return (double)left - (double)right;
      case TokenType.Plus:
        return (double)left + (double)right;
      case TokenType.Slash:
        return (double)left / (double)right;
      case TokenType.Asterisk:
        return (double)left * (double)right;
      default:
        throw new Exception("Unreachable");
    }
  }

  public object VisitGroupingExpression(Expr.Grouping expr)
  {
    return Evaluate(expr.Expr);
  }

  public object VisitLiteralExpression(Expr.Literal expr)
  {
    return expr.LiteralValue ?? throw new ArgumentNullException();
  }

  public object VisitUnaryExpression(Expr.Unary expr)
  {
    var right = Evaluate(expr.Right);
    switch (expr.Op.TokenType)
    {
      case TokenType.Minus:
        return -(double)right;
      default:
        throw new Exception("Unreachable");
    }
  }

  private object Evaluate(Expr expr)
  {
    return expr.Accept(this);
  }

  public object Interpret(Expr expr)
  {
    return Evaluate(expr);
  }
}
