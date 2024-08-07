using calculator;

var input = "-2 * 8 / 4";

var tokens = Lexer.Tokenize(input);
foreach (var token in tokens)
{
    Console.WriteLine(token);
}

var parser = new Parser(tokens);
var ast = parser.Parse();


Console.WriteLine(AstPrinter.ToString(ast));

var instructions = Compiler.Compile(ast);
var vm = new VirtualMachine(instructions);
vm.Run();
// vm.RunAndPrintStack();
vm.PrintStackTop();