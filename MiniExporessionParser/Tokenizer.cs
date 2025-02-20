namespace MiniExpressionParser;

public class Tokenizer
{
  public List<Token> Tokens { get; private set; } = [];

  private readonly string _text;
  private int _start = 0;
  private int _current = 0;
  private int _line = 1;

  public Tokenizer(string text)
  {
    _text = text;
  }

  public List<Token> Scan()
  {
    while (!IsAtEnd())
    {
      _start = _current;
      ScanToken();
    }

    AddToken(TokenType.EOF);

    return Tokens;
  }

  private bool IsAtEnd()
  {
    return _current >= _text.Length;
  }

  private void ScanToken()
  {
    var c = Advance();
    switch (c)
    {
      case '(':
        AddToken(TokenType.LeftParen);
        break;
      case ')':
        AddToken(TokenType.RightParen);
        break;
      case '+':
        AddToken(TokenType.Plus);
        break;
      case '-':
        AddToken(TokenType.Minus);
        break;
      case '/':
        AddToken(TokenType.Slash);
        break;
      case '*':
        AddToken(TokenType.Asterisk);
        break;
      case ' ':
      case '\r':
      case '\t':
        // Ignore whitespace.
        break;
      default:
        if (IsDigit(c))
        {
          Number();
        }
        else
        {
          throw new Exception($"Unsupported token {c}");
        }
        break;
    }
  }

  private char? PeekNext()
  {
    if (_current + 1 >= _text.Length)
    {
      return null;
    }
    return _text[_current + 1];
  }

  private bool IsDigit(char? c)
  {
    if (c is null)
    {
      throw new ArgumentNullException();
    }

    return c >= '0' && c <= '9';
  }

  private void Number()
  {
    while (IsDigit(Peek()))
    {
      Console.WriteLine(Peek());
      Advance();
    }

    if (Peek() == '.' && IsDigit(PeekNext()))
    {
      Advance();
      while (IsDigit(Peek()))
      {
        Advance();
      }
    }

    AddToken(TokenType.Number, Convert.ToDouble(_text[_start.._current]));
  }

  private char? Peek()
  {
    if (IsAtEnd())
    {
      return null;
    }
    return _text[_current];
  }

  private bool Match(char expected)
  {
    if (IsAtEnd())
    {
      return false;
    }

    if (_text[_current] != expected)
    {
      return false;
    }
    _current++;
    return true;
  }

  private char Advance()
  {
    return _text[_current++];
  }

  private void AddToken(TokenType tokenType)
  {
    AddToken(tokenType, null);
  }

  private void AddToken(TokenType tokenType, object? literal)
  {
    string lexeme = _text[_start.._current];
    Tokens.Add(new(tokenType, lexeme, _line, 0, literal));
  }
}
