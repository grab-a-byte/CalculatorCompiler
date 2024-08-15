namespace calculator;

public static class Compiler
{
    public static IEnumerable<Operation> Compile(Expression expr)
    {
        if (expr is NumberLiteral nl)
        {
            return[new Push(nl.Number)];
        }
        else if (expr is PrefixExpression pe)
        {
            if(pe.Op is '-')
            {
                return  [.. Compile(pe.Right) ,new Negate()];
            }
            else
            {
                throw new Exception("Unknown prefox expression");
            }
        }
        else if (expr is InfixExpression ie)
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

        return [];
    }
}
