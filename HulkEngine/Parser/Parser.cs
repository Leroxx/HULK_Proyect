
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
            switch (type)
            {
                case Token.TokenType.LPAREN:
                    throw new ArgumentException("Syntax Error: Missing '('");
                case Token.TokenType.RPAREN:
                    throw new ArgumentException("Syntax Error: Missing ')'");
                case Token.TokenType.IN:
                    throw new ArgumentException("Syntax Error: Missing body of the expression 'let-in'");
                case Token.TokenType.LAMBDA:
                    throw new ArgumentException("Syntax Error: Missing '=>' on function declaration");
                case Token.TokenType.ELSE:
                    throw new ArgumentException("Syntax Error: Missing 'else block' of the expression 'if-else'");
                case Token.TokenType.ASSIGN:
                    throw new ArgumentException("Syntax Error: Missing '=' on variable declaration");
                case Token.TokenType.COMMA:
                    throw new ArgumentException("Syntax Error: Missing ',' on function declaration");
                default:
                    break;
            }
        }

        // Consumes a token and gets the next one
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

        // Identifying the type of program.
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
            else if (token.Type == Token.TokenType.IF)
            {
                Eat(Token.TokenType.IF);
                node = IfStatement();
            }
            else
            {
                node = Expr();
            }

            return node;
        }

        // Getting a print node
        private AST PrintStatement()
        {
            Token token = current_token;

            Eat(Token.TokenType.LPAREN);
            Print node = new(Statement());
            Eat(Token.TokenType.RPAREN);

            return node;
        }

        // Getting a function node
        private AST FunctionStatement()
        {
            Token token = current_token;
            string name = "";
            LinkedList<AST> var_list = new LinkedList<AST>();
            AST node;

            if (current_token.Type == Token.TokenType.ID)
                Eat(Token.TokenType.ID);
            else
               throw new ArgumentException("Syntax Error: Missing function name"); 

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

            Token.Functions.Add(name);
            FunctionDeclaration function_node = new(name, var_list, Statement());
            return function_node;
        }

        // Getting a let-in expression node
        private AST LetInStatement()
        {
            Token token = current_token;
            LinkedList<AST> var_list = new LinkedList<AST>();
            AST node;

            while (current_token.Type != Token.TokenType.IN)
            {
                if (current_token.Type == Token.TokenType.COMMA)
                    Eat(Token.TokenType.COMMA);

                if (current_token.Type == Token.TokenType.EOL)
                    Eat(Token.TokenType.IN);

                node = Assign();
                var_list.AddLast(node);
            }

            Eat(Token.TokenType.IN);
            LetIN let_node = new(var_list, Statement());
            return let_node;
        }

        // Getting a if-else expression node
        private AST IfStatement()
        {
            AST node_condition;
            AST if_block;

            Eat(Token.TokenType.LPAREN);
            node_condition = Statement();
            Eat(Token.TokenType.RPAREN);

            if_block = Statement();
            Eat(Token.TokenType.ELSE);

            IfElse node = new(if_block, node_condition, Statement());

            return node;
        }

        // Variable declaration
        private AST Assign()
        {
            AST left = Var();
            Token token = current_token;
            Eat(Token.TokenType.ASSIGN);
            AST right = Statement();
            Assign node = new(left, token, right);
            return node;
        }

        // Variable node
        private AST Var()
        {
            Var node = new(current_token);

            if (current_token.Type == Token.TokenType.ID)
                Eat(Token.TokenType.ID);
            else
               throw new ArgumentException("Syntax Error: Missing variable name");

            return node;
        }

        // Math functions node
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

        // Functions node of a function which has already been declared
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

        /* Returns the nodes of the unary operators, negation and the three allowed data types (number, bool, string), 
          as well as the nodes of the mathematical functions, declared function calls, constants and variables. */
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
            else if(token.Type == Token.TokenType.NEGATION)
            {
                Eat(Token.TokenType.NEGATION);
                Negation node = new(token, Factor());
                return node;
            }
            else if (token.Type == Token.TokenType.NUMBER)
            {
                Eat(Token.TokenType.NUMBER);
                return new Num(token);
            }
            else if (token.Type == Token.TokenType.STRING)
            {
                String node = new(token);
                Eat(Token.TokenType.STRING);
                return node;
            }
            else if (token.Type == Token.TokenType.TRUE || token.Type == Token.TokenType.FALSE)
            {
                Bool node = new(token);
                if (token.Type == Token.TokenType.TRUE)
                    Eat(Token.TokenType.TRUE);
                else
                    Eat(Token.TokenType.FALSE);

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

        // Returns the nodes of the highest priority binary operators as well as comparison operators.
        private AST Term()
        {
            AST node = Factor();

            if (current_token.Type == Token.TokenType.POW)
            {
                Eat(Token.TokenType.POW);
                node = new Pow(node, Factor());
                return node;
            }

            while (current_token.Type == Token.TokenType.MUL || current_token.Type == Token.TokenType.DIV || current_token.Type == Token.TokenType.MODULE)
            {
                Token token = current_token;

                if (token.Type == Token.TokenType.MUL)
                    Eat(Token.TokenType.MUL);
                else if (token.Type == Token.TokenType.DIV)
                    Eat(Token.TokenType.DIV);
                else if (token.Type == Token.TokenType.MODULE)
                    Eat (Token.TokenType.MODULE);

                node = new BinOP(node, token, Factor());
            }

            if (current_token.Type == Token.TokenType.LESS_THAN ||
                current_token.Type == Token.TokenType.GREATER_THAN ||
                current_token.Type == Token.TokenType.LESS_THAN_OR_EQUAL ||
                current_token.Type == Token.TokenType.GREATER_THAN_OR_EQUAL ||
                current_token.Type == Token.TokenType.EQUAL ||
                current_token.Type == Token.TokenType.NOT_EQUAL)
            {
                Token token = current_token;

                if (token.Type == Token.TokenType.LESS_THAN)
                    Eat(Token.TokenType.LESS_THAN);
                else if (token.Type == Token.TokenType.GREATER_THAN)
                    Eat(Token.TokenType.GREATER_THAN);
                else if (token.Type == Token.TokenType.LESS_THAN_OR_EQUAL)
                    Eat(Token.TokenType.LESS_THAN_OR_EQUAL);
                else if (token.Type == Token.TokenType.GREATER_THAN_OR_EQUAL)
                    Eat(Token.TokenType.GREATER_THAN_OR_EQUAL);
                else if (token.Type == Token.TokenType.EQUAL)
                    Eat(Token.TokenType.EQUAL);
                else if (token.Type == Token.TokenType.NOT_EQUAL)
                    Eat(Token.TokenType.NOT_EQUAL);

                node = new LogicOP(node, token, Factor());
            }

            return node;
        }

        // Returns the nodes of the lowest priority binary operators as well as Boolean operators.
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

            while (current_token.Type == Token.TokenType.AND || current_token.Type == Token.TokenType.OR)
            {
                Token token = current_token;

                if (token.Type == Token.TokenType.AND)
                {
                    Eat(Token.TokenType.AND);
                    node = new ANDNode(node, Term());
                }
                else if (token.Type == Token.TokenType.OR)
                {
                    Eat(Token.TokenType.OR);
                    node = new ORNode(node, Term());
                }
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
