namespace MiniExpressionParser;

public class Token
{
  public TokenType TokenType { get; } = TokenType.Unknown;
  public string Lexeme { get; } = string.Empty;
  public object? Literal { get; }
  public int Line { get; }
  public int Pos { get; }

  public Token(TokenType tokenType, string lexeme, int line, int pos, object? literal = null)
  {
    TokenType = tokenType;
    Lexeme = lexeme;
    Line = line;
    Pos = pos;
    Literal = literal;
  }
}
