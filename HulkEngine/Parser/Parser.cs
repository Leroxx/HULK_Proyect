
namespace HulkEngine
{
    public class Parser
    {
        Token current_token;

        public Parser(Lexer lexer)
        {
            this.Lexer = lexer;
            current_token = Lexer.GetNexToken();
        }

        private Lexer Lexer { get; set; }

        private void Error(Token.TokenType type)
        {
            throw new ArgumentException(String.Format("! SYNTAX ERROR : Missing '{0}'", type), "type");
        }

        private void Eat(Token.TokenType type)
        {   
            if (current_token is not null)
            {
                if (current_token.Type == type)
                    current_token = Lexer.GetNexToken();
                else
                    Error(type);
            }
        }

        private AST Program()
        {
            AST node = Statement();
            Eat(Token.TokenType.EOL);

            return node;
        }

        private AST Statement()
        {
            AST node;
            Token token = current_token;

            if (token.Type == Token.TokenType.PRINT)
            {
                Eat(Token.TokenType.PRINT);
                node = PrintStatement();
            }
            else if (token.Type == Token.TokenType.LET)
            {
                Eat(Token.TokenType.LET);
                node = LetInStatement();
            }
            else
            {
                node = Expr();
            }

            return node;
        }

        private AST PrintStatement()
        {
            Token token = current_token;

            Eat(Token.TokenType.LPAREN);
            Print node = new(Statement());
            Eat(Token.TokenType.RPAREN);

            return node;
        }

        private AST LetInStatement()
        {
            Token token = current_token;
            LinkedList<AST> var_list = new LinkedList<AST>();
            AST node;

            while (current_token.Type != Token.TokenType.IN)
            {
                if (current_token.Type == Token.TokenType.COMMA)
                    Eat(Token.TokenType.COMMA);

                node = Assign();
                var_list.AddLast(node);
            }

            Eat(Token.TokenType.IN);
            LetIN let_node = new(var_list, Statement());
            return let_node;
        }

        private AST Assign()
        {
            AST left = Var();
            Token token = current_token;
            Eat(Token.TokenType.ASSIGN);
            AST right = Expr();
            Assign node = new(left, token, right);
            return node;
        }

        private AST Var()
        {
            Var node = new(current_token);
            Eat(Token.TokenType.ID);
            return node;
        }

        private AST Factor()
        {
            Token token = current_token;

            if (token.Type == Token.TokenType.PLUS)
            {
                Eat(Token.TokenType.PLUS);
                UnaryOP node = new(token, Factor());
                return node;
            }
            else if (token.Type == Token.TokenType.MINUS)
            {
                Eat(Token.TokenType.MINUS);
                UnaryOP node = new(token, Factor());
                return node;
            }
            else if (token.Type == Token.TokenType.INTEGRER)
            {
                Eat(Token.TokenType.INTEGRER);
                return new Num(token);
            }
            else if (token.Type == Token.TokenType.LPAREN)
            {
                Eat(Token.TokenType.LPAREN);
                AST node = Expr();
                Eat(Token.TokenType.RPAREN);
                return node;
            }
            else 
            {
                AST node = Var();
                return node;
            }
        }

        private AST Term()
        {
            AST node = Factor();

            while (current_token.Type == Token.TokenType.MUL || current_token.Type == Token.TokenType.DIV)
            {
                Token token = current_token;

                if (token.Type == Token.TokenType.MUL)
                    Eat(Token.TokenType.MUL);
                else if (token.Type == Token.TokenType.DIV)
                    Eat(Token.TokenType.DIV);

                node = new BinOP(node, token, Factor());
            }

            return node;
        }

        public AST Expr()
        {   
            AST node = Term();

            while (current_token.Type == Token.TokenType.PLUS || current_token.Type == Token.TokenType.MINUS)
            {
                Token token = current_token;

                if (token.Type == Token.TokenType.PLUS)
                    Eat(Token.TokenType.PLUS);
                else if (token.Type == Token.TokenType.MINUS)
                    Eat(Token.TokenType.MINUS);

                node = new BinOP(node, token, Term());
            }

            return node;
        }


        public AST Parse()
        {
            AST node = Program();
            return node;
        }
    }
}
