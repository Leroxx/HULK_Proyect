
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

        public string Text { get; set; }

        private int pos = 0;

        private void Error(string messege)
        {
            throw new ArgumentException("Error Lexico: {0} no es un Token validos", messege);
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

        private char Peek()
        {
            int peek_pos = pos + 1;
            if (peek_pos > Text.Length - 1)
                return ' ';
            else
                return Text[peek_pos];
        }

        private string Number()
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

        private string String()
        {
            string result = "";

            while (current_char != '"')
            {
                result += current_char;
                Advance();
            }
            
            Advance();
            return result;
        }

        public Token ID()
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
            else if (Token.MathFunction.ContainsKey(result))
            {
                token = Token.MathFunction[result];
            }
            else if (Token.Constant.ContainsKey(result))
            {
                token = Token.Constant[result];
            }
            else if (Token.Functions.Contains(result))
            {
                token = new Token(Token.TokenType.FUNCTION_CALL, result);
            }
            else if (result == "true" || result == "false")
            {
                if (result == "true")
                    token = new Token(Token.TokenType.TRUE, "true");
                else
                    token = new Token(Token.TokenType.FALSE, "false");
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
                    return new Token(Token.TokenType.NUMBER, Number());

                if (char.IsLetterOrDigit(current_char))
                    return ID();

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

                if (current_char == '%')
                {
                    Advance();
                    return new Token(Token.TokenType.MODULE, "%");
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

                if (current_char == '^')
                {
                    Advance();
                    return new Token(Token.TokenType.POW, "^");
                }

                if (current_char == '=')
                {
                    if (Peek() == '>')
                    {
                        Advance(); Advance();
                        return new Token(Token.TokenType.LAMBDA, "=>");
                    }
                    else if (Peek() == '=')
                    {
                        Advance(); Advance();
                        return new Token(Token.TokenType.EQUAL, "==");
                    }
                    else
                    {
                        Advance();
                        return new Token(Token.TokenType.ASSIGN, "=");
                    }
                }

                if (current_char == ',')
                {
                    Advance();
                    return new Token(Token.TokenType.COMMA, ",");
                }

                if (current_char == '"')
                {
                    Advance();
                    return new Token(Token.TokenType.STRING, String());
                }

                if (current_char == '<')
                {
                    if (Peek() == '=')
                    {
                        Advance(); Advance();
                        return new Token(Token.TokenType.LESS_THAN_OR_EQUAL, "<=");
                    }
                    else
                    {
                        Advance();
                        return new Token(Token.TokenType.LESS_THAN, "<");
                    }
                }

                if (current_char == '>')
                {
                    if (Peek() == '=')
                    {
                        Advance(); Advance();
                        return new Token(Token.TokenType.GREATER_THAN_OR_EQUAL, ">=");
                    }
                    else
                    {
                        Advance();
                        return new Token(Token.TokenType.GREATER_THAN, ">");
                    }
                }

                if (current_char == '!')
                {
                    if (Peek() == '=')
                    {
                        Advance(); Advance();
                        return new Token(Token.TokenType.NOT_EQUAL, "!=");
                    }
                    else
                    {
                        Advance();
                        return new Token(Token.TokenType.NEGATION, "!");
                    }
                }

                if (current_char == '&')
                {
                    Advance();
                    return new Token(Token.TokenType.AND, "&");
                }

                if (current_char == '|')
                {
                    Advance();
                    return new Token(Token.TokenType.OR, "|");
                }

                Error(current_char.ToString());
            }

            return new Token(Token.TokenType.EOL, ";");
        }
    }
}
