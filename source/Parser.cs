using System.ComponentModel;

namespace calculator;

public record Expression();
public sealed record NumberLiteral(float Number, string Literal) : Expression;
public sealed record PrefixExpression(char Op, Expression Right) : Expression;
public sealed record InfixExpression(Expression Left, char Op, Expression Right) : Expression;

enum Precedence {
    Lowest = 0,
    Subtract = 1,
    Add = 2,
    Multiply = 3,
    Divide= 4,
    Prefix = 5
}

public sealed class Parser
{

    private int _current = 0;
    private readonly Token[] _tokens;

    private readonly Dictionary<TokenType, Func<Expression>> _prefixFunctions = [];
    private readonly Dictionary<TokenType, Func<Expression, Expression>> _infixFunctions = [];

    private static readonly Dictionary<TokenType, Precedence> _tokenPrecedences = new()
    {
        {TokenType.Hyphen, Precedence.Subtract},
        {TokenType.Plus, Precedence.Add},
        {TokenType.Star, Precedence.Multiply},
        {TokenType.Slash, Precedence.Divide},
    };

    public Parser(Token[] tokens)
    {
        _tokens = tokens;
        _prefixFunctions.Add(TokenType.Number, ParseNumber);
        _prefixFunctions.Add(TokenType.Hyphen, ParseNegation);

        _infixFunctions.Add(TokenType.Plus, ParseInfixExpression);
        _infixFunctions.Add(TokenType.Hyphen, ParseInfixExpression);
        _infixFunctions.Add(TokenType.Star, ParseInfixExpression);
        _infixFunctions.Add(TokenType.Slash, ParseInfixExpression);
    }

    public Expression Parse()
    {
        var expression = ParseExpression(Precedence.Lowest);
        return expression;
    }

    private Token? CurrentToken() => _current < _tokens.Length ? _tokens[_current] : null;
    private TokenType? CurrentTokenType() => CurrentToken()?.Type;
    private Precedence CurrentPrecedence()
    {
        var currentType = CurrentTokenType();
        return currentType is null ? Precedence.Lowest : _tokenPrecedences[(TokenType)currentType];
    }

    private Expression ParseNegation()
    {
        _current++;
        var right = ParseExpression(Precedence.Prefix);
        return new PrefixExpression('-',  right);
    }

    private Expression ParseExpression(Precedence precedence)
    {
        var currentToken = CurrentToken();
        var prefix = _prefixFunctions[currentToken!.Type] ?? throw new Exception("Unable to find prefix function");
        Expression left = prefix();
        var currentType = CurrentTokenType();
        if (currentType is null)
        {
            return left;
        }

        Precedence nextPrecedence = CurrentPrecedence();
        while (precedence < nextPrecedence)
        {
            var infix = _infixFunctions[(TokenType)CurrentTokenType()!];
            left = infix(left);
            nextPrecedence = CurrentPrecedence();
        }

        return left;
    }

    private InfixExpression ParseInfixExpression(Expression left)
    {
        var literal = CurrentToken()!.Literal;
        var precedence = CurrentPrecedence();
        _current++;

        var right = ParseExpression(precedence);
        return new InfixExpression(left, literal[0], right);
    }

    private NumberLiteral ParseNumber()
    {
        var tok = CurrentToken();
        _current++;
        var value = float.Parse(tok!.Literal);
        return new(value, tok.Literal);
    }

}
