namespace calculator;

public static class AstPrinter
{
    public static string ToString(Expression expr)
    {
        return expr switch
        {
            NumberLiteral nl => nl.Literal,
            PrefixExpression pe => "(" + pe.Op + ToString(pe.Right) + ")",
            InfixExpression ie => "(" + ToString(ie.Left) + ie.Op + ToString(ie.Right) + ")",
            _ => ""
        };
    }
}
