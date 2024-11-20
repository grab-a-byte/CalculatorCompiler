using calculator;

const string input = "1 + 2 - 3";

var tokens = Lexer.Tokenize(input);
foreach (var token in tokens)
{
    Console.WriteLine(token);
}

var parser = new Parser(tokens);
var ast = parser.Parse();


Console.WriteLine(AstPrinter.ToString(ast));

var instructions = Compiler.Compile(ast);
foreach (var instruction in instructions)
{
    Console.WriteLine(instruction);
}
var vm = new VirtualMachine(instructions);
// vm.Run();
vm.RunAndPrintStack();
vm.PrintStackTop();

// IlCompiler ilCompiler = new(ast);
// ilCompiler.Compile();
// ilCompiler.Save();