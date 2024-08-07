using calculator;

// var input = "1.1 + 2 - 3 / 3 + 4 * 2 + 1";
var input = "-2 * 4";

var tokens = Lexer.Tokenize(input);
foreach (var token in tokens)
{
    Console.WriteLine(token);
}

var parser = new Parser(tokens);
var ast = parser.Parse();

// Console.WriteLine(ast);

Console.WriteLine(AstPrinter.ToString(ast));

var instructions = Compiler.Compile(ast);
var vm = new VirtualMachine(instructions);
vm.RunAndPrintStack();
vm.PrintStackTop();