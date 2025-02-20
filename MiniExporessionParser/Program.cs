using MiniExpressionParser;

string source = "7 * 10 * (4 + 3)";

Tokenizer tokenizer = new(source);
List<Token> tokens = tokenizer.Scan();
Parser parser = new(tokens);
Expr expr = parser.Parse();

AstPrinter printer = new();
string ast = printer.Print(expr);
Console.WriteLine(ast);

Interpreter interpreter = new();
var value = interpreter.Interpret(expr);
Console.WriteLine(value);
