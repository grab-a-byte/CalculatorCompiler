namespace calculator;

public static class AstPrinter
{
    public static string ToString(Expression expr)
    {
        if (expr is NumberLiteral nl)
        {
            return nl.Literal;
        }
        else if (expr is PrefixExpression pe)
        {
            return "(" +  pe.Op.ToString() + ToString(pe.Right) + ")";
        }
        else if (expr is InfixExpression ie)
        {
            return "(" + ToString(ie.Left) + ie.Op.ToString() + ToString(ie.Right) + ")";
        }

        else return "";
    }
}
