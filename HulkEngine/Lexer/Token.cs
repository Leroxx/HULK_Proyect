
namespace HulkEngine
{
    public class Token
    {
        public enum TokenType
        {
            NUMBER,
            STRING,
            PLUS,
            MINUS,
            MUL,
            DIV,
            POW,
            LPAREN,
            RPAREN,
            PRINT,
            CONCATENATION,
            FUNCTION,
            FUNCTION_CALL,
            LAMBDA,
            LET,
            IN,
            ID,
            ASSIGN,
            COMMA,
            SQRT,
            SIN,
            COS,
            EXP,
            LOG,
            PI,
            E,
            EOL
        }

        public Token(TokenType type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public TokenType Type { get; set; }
        public string Value { get; set; }

        public static Dictionary<string, Token> Reserved_Keywords = new Dictionary<string, Token>()
        {
            { "print", new Token(TokenType.PRINT, "print") },
            { "function", new Token(TokenType.FUNCTION, "function")},
            { "let", new Token(TokenType.LET, "let")},
            { "in", new Token(TokenType.IN, "in")}
        };

        public static Dictionary<string, Token> MathFunction = new Dictionary<string, Token>()
        {
            { "sqrt", new Token (TokenType.SQRT , "sqrt")},
            { "sin", new Token (TokenType.SIN , "sin")},
            { "cos", new Token (TokenType.COS , "cos")},
            { "exp", new Token (TokenType.EXP , "exp")},
            { "log", new Token (TokenType.LOG , "log")}
        };

        public static List<string> Functions = new List<string>();

        public static Dictionary<string, Token> Constant = new Dictionary<string, Token>()
        {
            {"PI", new Token(TokenType.PI, "PI")},
            {"E", new Token(TokenType.E, "E")}
        };

        public override string ToString()
        {
            return "Token("+this.Type+","+this.Value+")";
        }
    }
}