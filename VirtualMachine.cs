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
        private Stack<float> _stack = new();

        public void Run()
        {
            foreach(var op in bytecode)
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
            }
        }

        public float StackTop() => _stack.First();
        public void PrintStackTop() => Console.WriteLine(StackTop());
}
