namespace MiniExpressionParser;

public enum TokenType
{
  Unknown = 0,
  Number,
  Identifier,
  LeftParen,
  RightParen,
  Slash,
  Asterisk,
  Plus,
  Minus,
  EOF,
}
