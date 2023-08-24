
namespace HulkEngine
{
    public class Token
    {
        public enum TokenType
        {
            INTEGRER,
            PLUS,
            MINUS,
            MUL,
            DIV,
            LPAREN,
            RPAREN,
            PRINT,
            LET,
            IN,
            ID,
            ASSIGN,
            COMMA,
            EOL
        }

        public static Dictionary<string, Token> Reserved_Keywords = new Dictionary<string, Token>()
        {
            { "print", new Token(TokenType.PRINT, "print") },
            { "let", new Token(TokenType.LET, "let")},
            { "in", new Token(TokenType.IN, "in")}
        };

        public Token(TokenType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public TokenType Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return "Token("+this.Type+","+this.Value+")";
        }
    }
}