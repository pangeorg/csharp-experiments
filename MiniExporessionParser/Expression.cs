namespace MiniExpressionParser;

public abstract class Expr
{
  public class Binary : Expr
  {
    public Expr Left { get; }
    public Expr Right { get; }
    public Token Op { get; }

    public Binary(Expr left, Token op, Expr right)
    {
      Left = left;
      Right = right;
      Op = op;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitBinaryExpression(this);
    }
  }

  public class Unary : Expr
  {
    public Expr Right { get; }
    public Token Op { get; }

    public Unary(Expr right, Token op)
    {
      Right = right;
      Op = op;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitUnaryExpression(this);
    }
  }

  public class Grouping : Expr
  {
    public Expr Expr { get; }

    public Grouping(Expr expr)
    {
      Expr = expr;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitGroupingExpression(this);
    }
  }

  public class Literal : Expr
  {
    public object LiteralValue { get; }

    public Literal(object literal)
    {
      LiteralValue = literal;
    }

    public override T Accept<T>(IVisitor<T> visitor)
    {
      return visitor.VisitLiteralExpression(this);
    }
  }

  public interface IVisitor<T>
  {
    T VisitBinaryExpression(Binary expr);
    T VisitGroupingExpression(Grouping expr);
    T VisitLiteralExpression(Literal expr);
    T VisitUnaryExpression(Unary expr);
  }

  public abstract T Accept<T>(IVisitor<T> visitor);
}
