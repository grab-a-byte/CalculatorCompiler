namespace calculator;

public enum TokenType
{
    Plus,
    Hyphen,
    Slash,
    Star,
    Number
}

public record Token(TokenType Type, string Literal);

public static class Lexer
{
    public static Token[] Tokenize(string input)
    {
        List<Token> tokens = [];
        int i = 0;
        while (i < input.Length)
        {
            i += SkipWhitespace(input[i..]);
            Token tok = input[i] switch
            {
                '+' => new(TokenType.Plus, "+"),
                '-' => new(TokenType.Hyphen, "-"),
                '*' => new(TokenType.Star, "*"),
                '/' => new(TokenType.Slash, "/"),
                _ => new(TokenType.Number, ParseNumber(input[i..]))
            };
            i += tok.Literal.Length;
            tokens.Add(tok);
        }

        return [.. tokens];
    }
    static string ParseNumber(string input) => new(input.TakeWhile(x => char.IsDigit(x) || x is '.').ToArray());
    static int SkipWhitespace(string input) => input.TakeWhile(char.IsWhiteSpace).Count();
}
