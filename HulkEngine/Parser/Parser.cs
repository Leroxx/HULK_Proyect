
namespace HulkEngine
{
    public class Parser
    {
        Token current_token;

        public Parser(Lexer lexer)
        {
            this.Lexer = lexer;
            current_token = lexer.GetNexToken();
        }

        public Lexer Lexer { get; set; }


        private void Error(Token.TokenType type)
        {
            throw new ArgumentException("! SYNTAX ERROR : Missing '{0}'", type.ToString());
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
            else if (token.Type == Token.TokenType.FUNCTION)
            {
                Eat(Token.TokenType.FUNCTION);
                node = FunctionStatement();
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

        private AST FunctionStatement()
        {
            Token token = current_token;
            string name = "";
            LinkedList<AST> var_list = new LinkedList<AST>();
            AST node;

            Eat(Token.TokenType.ID);
            name = token.Value;

            Eat(Token.TokenType.LPAREN);

            while (current_token.Type != Token.TokenType.RPAREN)
            {
                if (current_token.Type == Token.TokenType.COMMA)
                    Eat(Token.TokenType.COMMA);

                node = Var();
                var_list.AddLast(node);
            }

            Eat(Token.TokenType.RPAREN);
            Eat(Token.TokenType.LAMBDA);

            FunctionDeclaration function_node = new(name, var_list, Statement());
            return function_node;
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

        private AST MathFunction(Token token)
        {
            if (token.Value == "sqrt" || token.Value == "sin" || token.Value == "cos" || token.Value == "exp")
            {
                Eat(Token.TokenType.LPAREN);
                MathFunction node = new(token, Expr());
                Eat(Token.TokenType.RPAREN);
                return node;
            }
            else
            {
                Eat(Token.TokenType.LPAREN);
                AST right = Expr();
                Eat(Token.TokenType.COMMA);
                AST left = Expr();
                Eat(Token.TokenType.RPAREN);
                LogFunction node = new(right, left);
                return node;
            }
        }

        private AST FunctionCall()
        {
            Token token = current_token;

            Eat(Token.TokenType.FUNCTION_CALL);
            Eat(Token.TokenType.LPAREN);
            List<AST> list = new List<AST>();

            while (current_token.Type != Token.TokenType.RPAREN)
            {   
                if (current_token.Type == Token.TokenType.COMMA)
                    Eat(Token.TokenType.COMMA);

                AST exp = Expr();
                list.Add(exp);
            }

            Eat(Token.TokenType.RPAREN);
            FunctionCall node = new(token.Value, list);
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
            else if (token.Type == Token.TokenType.NUMBER)
            {
                Eat(Token.TokenType.NUMBER);
                return new Num(token);
            }
            else if (token.Type == Token.TokenType.STRING)
            {
                String node = new(current_token);
                Eat(Token.TokenType.STRING);
                return node;
            }
            else if (token.Type == Token.TokenType.LPAREN)
            {
                Eat(Token.TokenType.LPAREN);
                AST node = Statement();
                Eat(Token.TokenType.RPAREN);
                return node;
            }
            else if (token.Type == Token.TokenType.FUNCTION_CALL)
            {
                AST node = FunctionCall();
                return node;
            }
            else if (Token.Constant.ContainsKey(token.Value))
            {
                Eat(token.Type);
                Constants node = new(token);
                return node;
            }
            else if (Token.MathFunction.ContainsKey(token.Value))
            {
                Eat(token.Type);
                AST node = MathFunction(token);
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

            if (current_token.Type == Token.TokenType.POW)
            {
                Eat(Token.TokenType.POW);
                node = new Pow(node, Factor());
                return node;
            }

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

            if (current_token.Type == Token.TokenType.CONCATENATION)
            {
                Eat(Token.TokenType.CONCATENATION);
                node = new Concatenation(node, Term());
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
