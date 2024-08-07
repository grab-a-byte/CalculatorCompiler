namespace calculator;

public record Operation();
public record Add() : Operation;
public record Subtract() : Operation;
public record Multiply() : Operation;
public record Divide() : Operation;
public record Negate() : Operation;
public record Push(float Value) : Operation;

public class VirtualMachine(IEnumerable<Operation> bytecode)
{
    private readonly Stack<float> _stack = new();

    public void Run()
    {
        foreach (var op in bytecode)
        {
            HandleOperation(op);
        }
    }

    public void RunAndPrintStack()
    {
        foreach (var op in bytecode)
        {
            HandleOperation(op);
            PrintArray(_stack.ToArray());
        }
    }

    private void HandleOperation(Operation op)
    {
            if (op is Push p)
            {
                _stack.Push(p.Value);
            }
            else if (op is Subtract)
            {
                var right = _stack.Pop();
                var left = _stack.Pop();
                _stack.Push(left - right);
            }
            else if (op is Add)
            {
                var right = _stack.Pop();
                var left = _stack.Pop();
                _stack.Push(left + right);
            }
            else if (op is Multiply)
            {
                var right = _stack.Pop();
                var left = _stack.Pop();
                _stack.Push(left * right);
            }
            else if (op is Divide)
            {
                var right = _stack.Pop();
                var left = _stack.Pop();
                _stack.Push(left / right);
            }
            else if (op is Negate)
            {
                _stack.Push(-_stack.Pop());
            }
    }
    public float StackTop() => _stack.First();
    public void PrintStackTop() => Console.WriteLine(StackTop());

    private static void PrintArray<T>(T[] array) => Console.WriteLine("[" + string.Join(",", array) + "]");
}
