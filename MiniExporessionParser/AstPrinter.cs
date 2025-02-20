using System.Text;

namespace MiniExpressionParser;

public class AstPrinter : Expr.IVisitor<string>
{
  public string VisitBinaryExpression(Expr.Binary expr)
  {
    return Parenthesize(expr.Op.Lexeme, expr.Left, expr.Right);
  }

  public string VisitGroupingExpression(Expr.Grouping expr)
  {
    return Parenthesize("group", expr.Expr);
  }

  public string VisitLiteralExpression(Expr.Literal expr)
  {
    if (expr.LiteralValue is null)
    {
      return "nil";
    }

    return expr.LiteralValue.ToString() ?? string.Empty;
  }

  public string VisitUnaryExpression(Expr.Unary expr)
  {
    return Parenthesize(expr.Op.Lexeme, expr.Right);
  }

  public string Print(Expr expr)
  {
    return expr.Accept(this);
  }

  private string Parenthesize(string name, params Expr[] exprs)
  {
    var sb = new StringBuilder();

    sb.Append("(");
    sb.Append(name);

    foreach (var expr in exprs)
    {
      sb.Append(" ");
      sb.Append(expr.Accept(this));
    }

    sb.Append(")");

    return sb.ToString();
  }
}
