using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;

namespace calculator;

public class IlCompiler
{
    private readonly Expression _expr;
    private PersistedAssemblyBuilder _assemblyBuilder;
    private ModuleBuilder _moduleBuilder;
    private TypeBuilder _typeBuilder;
    private MethodBuilder _methodBuilder;

    private ILGenerator _ilGenerator;

    public IlCompiler(Expression expr)
    {
        _expr = expr;
        _assemblyBuilder = new PersistedAssemblyBuilder(new AssemblyName("Calculator"), typeof(object).Assembly);
        _moduleBuilder = _assemblyBuilder.DefineDynamicModule("CalculatorModule");
        _typeBuilder = _moduleBuilder.DefineType("Program", TypeAttributes.Public);
        _methodBuilder = _typeBuilder.DefineMethod("Main",
            MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static);
        _ilGenerator = _methodBuilder.GetILGenerator();
    }

    public void Compile() => Compile(_expr);

    private void Compile(Expression expr)
    {
        switch (expr)
        {
            case NumberLiteral nl:
            {
                _ilGenerator.Emit(OpCodes.Ldc_R8, nl.Number);
                break;
            }
            case PrefixExpression { Op: '-' } pe:
            {
                Compile(pe.Right);
                _ilGenerator.Emit(OpCodes.Neg);
                break;
            }
            case PrefixExpression pe:
                throw new Exception("Unknown prefix expression");
            case InfixExpression ie:
            {
                Compile(ie.Right);
                Compile(ie.Left);
                switch (ie.Op)
                {
                    case '+':
                        _ilGenerator.Emit(OpCodes.Add);
                        break;
                    case '-':
                        _ilGenerator.Emit(OpCodes.Sub);
                        break;
                    case '/':
                        _ilGenerator.Emit(OpCodes.Div);
                        break;
                    case '*':
                        _ilGenerator.Emit(OpCodes.Mul);
                        break;
                }

                break;
            }
        }
    }

    public void Save()
    {
        _typeBuilder.CreateType();

        MetadataBuilder metadataBuilder = _assemblyBuilder.GenerateMetadata(out BlobBuilder ilStream, out BlobBuilder fieldData);
        PEHeaderBuilder peHeaderBuilder = new PEHeaderBuilder(imageCharacteristics: Characteristics.ExecutableImage);

        ManagedPEBuilder peBuilder = new ManagedPEBuilder(
            header: peHeaderBuilder,
            metadataRootBuilder: new MetadataRootBuilder(metadataBuilder),
            ilStream: ilStream,
            mappedFieldData: fieldData,
            entryPoint: MetadataTokens.MethodDefinitionHandle(_methodBuilder.MetadataToken));

        BlobBuilder peBlob = new BlobBuilder();
        peBuilder.Serialize(peBlob);

        // in case saving to a file:
        using var fileStream = new FileStream("MyAssembly.exe", FileMode.Create, FileAccess.Write);
        peBlob.WriteContentTo(fileStream);
    }
}