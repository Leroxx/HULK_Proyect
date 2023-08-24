
using System.Globalization;
using System.Security.Cryptography;

namespace HulkEngine
{
    public class Lexer
    {
        private char current_char;

        public Lexer(string text)
        {
            this.Text = text;
            current_char = Text[pos];
        }

        private string Text { get; set; }
        private int pos = 0;

        private void Error(string text)
        {
            throw new ArgumentException(String.Format("! LEXICAL ERROR : '{0}' is not valid Token", text), "text");
        }

        private void Advance()
        {
            pos += 1;
            if (current_char != ';' && pos <= Text.Length - 1)
                current_char = Text[pos];
            else
                throw new Exception("falta ;");
        }

        private void SkipWhitSpaces()
        {
            while (current_char != ';' && char.IsWhiteSpace(current_char))
                Advance();
        }

        private string Integrer()
        {
            string result = "";
            while (current_char != ';' && char.IsDigit(current_char))
            {
                result += current_char;
                Advance();
            }

            if (current_char == '.')
            {
                result += current_char;
                Advance();

                while (current_char != ';' && char.IsDigit(current_char))
                {
                    result += current_char;
                    Advance();
                }
            }
            
            return result;
        }

        public Token _ID()
        {
            string result = "";

            while (current_char != ';' && char.IsLetterOrDigit(current_char))
            {
                result += current_char;
                Advance();
            }

            Token token;

            if (Token.Reserved_Keywords.ContainsKey(result))
            {
                token = Token.Reserved_Keywords[result];
            }
            else 
            {
                token = new Token(Token.TokenType.ID, result);
            }
          
            return token;
        }

        public Token GetNexToken()
        {
            while (current_char != ';')
            {
                if (char.IsWhiteSpace(current_char))
                {
                    SkipWhitSpaces();
                    continue;
                }

                if (char.IsDigit(current_char))
                    return new Token(Token.TokenType.INTEGRER, Integrer());

                if (char.IsLetterOrDigit(current_char))
                    return _ID();

                if (current_char == '+')
                {
                    Advance();
                    return new Token(Token.TokenType.PLUS, "+");
                }

                if (current_char == '-')
                {
                    Advance();
                    return new Token(Token.TokenType.MINUS, "-");
                }

                if (current_char == '*')
                {
                    Advance();
                    return new Token(Token.TokenType.MUL, "*");
                }

                if (current_char == '/')
                {
                    Advance();
                    return new Token(Token.TokenType.DIV, "/");
                }

                if (current_char == '(')
                {
                    Advance();
                    return new Token(Token.TokenType.LPAREN, "(");
                }

                if (current_char == ')')
                {
                    Advance();
                    return new Token(Token.TokenType.RPAREN, ")");
                }

                if (current_char == '=')
                {
                    Advance();
                    return new Token(Token.TokenType.ASSIGN, "=");
                }

                if (current_char == ',')
                {
                    Advance();
                    return new Token(Token.TokenType.COMMA, ",");
                }

                Error(current_char.ToString());
            }

            return new Token(Token.TokenType.EOL, ";");
        }
    }
}
