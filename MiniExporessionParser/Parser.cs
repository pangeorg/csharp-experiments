namespace MiniExpressionParser;

public class Parser
{
  private readonly List<Token> _tokens;
  private int _current = 0;

  public Parser(List<Token> tokens)
  {
    _tokens = tokens;
  }

  public Expr Parse()
  {
    return Expression();
  }

  private bool IsAtEnd()
  {
    return _current >= _tokens.Count;
  }

  private Token Peek()
  {
    return _tokens[_current];
  }

  private Token Advance()
  {
    return _tokens[_current++];
  }

  private Token Previous()
  {
    return _tokens[_current - 1];
  }

  private bool Match(params TokenType[] tokenTypes)
  {
    foreach (var tokenType in tokenTypes)
    {
      if (Check(tokenType))
      {
        Advance();
        return true;
      }
    }
    return false;
  }


  private bool Check(TokenType tokenType)
  {
    if (IsAtEnd()) return false;
    return Peek().TokenType == tokenType;
  }

  private Expr Expression()
  {
    return Term();
  }

  private Expr Term()
  {

    Expr expr = Factor();

    while (Match(TokenType.Plus, TokenType.Minus))
    {
      Token op = Previous();
      Expr right = Factor();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr Factor()
  {
    Expr expr = Unary();

    while (Match(TokenType.Slash, TokenType.Asterisk))
    {
      Token op = Previous();
      Expr right = Unary();
      expr = new Expr.Binary(expr, op, right);
    }

    return expr;
  }

  private Expr Unary()
  {
    if (Match(TokenType.Minus))
    {
      Token op = Previous();
      Expr right = Unary();
      return new Expr.Unary(right, op);
    }

    return Primary();
  }

  private Token Consume(TokenType tokenType, string message)
  {
    if (Check(tokenType))
    {
      return Advance();
    }
    throw new Exception(message);
  }

  private Expr Primary()
  {
    if (Match(TokenType.Number))
    {
      return new Expr.Literal(Previous().Literal);
    }

    if (Match(TokenType.LeftParen))
    {
      Expr expr = Expression();
      Consume(TokenType.RightParen, "Expected ')' after Expression");
      return new Expr.Grouping(expr);
    }

    throw new Exception("Unhandled Case");
  }

}
