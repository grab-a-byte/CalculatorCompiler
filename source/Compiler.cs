namespace calculator;

public static class Compiler
{
    public static IEnumerable<Operation> Compile(Expression expr)
    {
        switch (expr)
        {
            case NumberLiteral nl:
                return[new Push(nl.Number)];
            case PrefixExpression { Op: '-' } pe:
                return  [.. Compile(pe.Right) ,new Negate()];
            case PrefixExpression pe:
                throw new Exception("Unknown prefix expression");
            case InfixExpression ie:
            {
                IEnumerable<Operation> left = Compile(ie.Left);
                IEnumerable<Operation> right = Compile(ie.Right);
                Operation operation = ie.Op switch
                {
                    '+' => new Add(),
                    '-' => new Subtract(),
                    '/' => new Divide(),
                    '*' => new Multiply(),
                    _ => throw new Exception("Unknown operation")
                };

                return [..left, ..right, operation];
            }
            default:
                return [];
        }
    }
}
