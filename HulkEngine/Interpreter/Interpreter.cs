
namespace HulkEngine
{
    public class Interpreter : NodeVisitor
    {
        public Interpreter(SymbolTable symbolTable)
        {
            this.SymbolTable = symbolTable;
        }

        public SymbolTable SymbolTable { get; set; }

        public void Error(string mes)
        {
            throw new ArgumentException(mes);
        }

        // Returns the result of the corresponding binary operation
        public dynamic Visit_BinOP(dynamic node)
        {
            if (node.OP.Type == Token.TokenType.PLUS)
            {
                return Visit(node.Left) + Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.MINUS)
            {
                return Visit(node.Left) - Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.MUL)
            {
                return Visit(node.Left) * Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.DIV)
            {
                return Visit(node.Left) / Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.MODULE)
            {
                return Visit(node.Left) % Visit(node.Right);
            }

            throw new Exception("Invalid operator");
        }

        // Returns the result of the corresponding comparators
        public dynamic Visit_LogicOP(dynamic node)
        {
            if (node.OP.Type == Token.TokenType.LESS_THAN)
            {
                return Visit(node.Left) < Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.GREATER_THAN)
            {
                return Visit(node.Left) > Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.LESS_THAN_OR_EQUAL)
            {
                return Visit(node.Left) <= Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.GREATER_THAN_OR_EQUAL)
            {
                return Visit(node.Left) >= Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.EQUAL)
            {
                return Visit(node.Left) == Visit(node.Right);
            }
            else if (node.OP.Type == Token.TokenType.NOT_EQUAL)
            {
                return Visit(node.Left) != Visit(node.Right);
            }

            throw new Exception("Invalid operator");
        }

        // Returns the result of Visit a ! negation node
        public dynamic Visit_Negation(dynamic node)
        {
            if (Visit(node.Expression))
                return false;
            else
                return true;
        }

        // Returns the evaluation of the expression
        public dynamic Visit_ANDNode(dynamic node)
        {
            if (Visit(node.Left) && Visit(node.Right))
                return true;
            else
                return false;
        }

        public dynamic Visit_ORNode(dynamic node)
        {
            if (Visit(node.Left) || Visit(node.Right))
                return true;
            else
                return false;
        }

        public dynamic Visit_MathFunction(dynamic node)
        {
            if (Visit(node.Expression) is string || Visit(node.Expression) is bool)
                throw new ArgumentException("Functions accept numbers only");

            if (node.Function.Type == Token.TokenType.SQRT)
            {
                return Math.Sqrt(Visit(node.Expression));
            }
            else if (node.Function.Type == Token.TokenType.SIN)
            {
                return Math.Sin(Visit(node.Expression));
            }
            else if (node.Function.Type == Token.TokenType.COS)
            {
                return Math.Cos(Visit(node.Expression));
            }
            else if (node.Function.Type == Token.TokenType.EXP)
            {
                return Math.Pow(Math.E, Visit(node.Expression));
            }

            throw new Exception("Invalid function");
        }

        public dynamic Visit_LogFunction(dynamic node)
        {
            if (Visit(node.Expression) is string || Visit(node.Expression_Base) is bool ||
                Visit(node.Expression) is string || Visit(node.Expression_Base) is bool)
                throw new ArgumentException("Functions accept numbers only");

            return Math.Log(Visit(node.Expression), Visit(node.Expression_Base));
        }

        public dynamic Visit_Pow(dynamic node)
        {
            if (Visit(node.Expression) is string || Visit(node.Exp) is bool ||
                Visit(node.Expression) is string || Visit(node.Exp) is bool)
                throw new ArgumentException("Functions accept numbers only");

            return Math.Pow(Visit(node.Expression), Visit(node.Exp));
        }

        public dynamic Visit_Num(dynamic node)
        {
            return node.Value;
        }

        public dynamic Visit_Constants(dynamic node)
        {
            if (node.Token.Type == Token.TokenType.PI)
                return Math.PI;
            else if (node.Token.Type == Token.TokenType.E)
                return Math.E;

            throw new Exception("Invalid constant");
        }

        public dynamic Visit_String(dynamic node)
        {
            return node.Value;
        }

        public dynamic Visit_Bool(dynamic node)
        {
            return node.Value;
        }

        public dynamic Visit_UnaryOP(dynamic node)
        {
            Token.TokenType type = node.TokenType;

            if (type == Token.TokenType.PLUS)
                return +Visit(node.Exp);
            else if (type == Token.TokenType.MINUS)
                return -Visit(node.Exp);

            throw new Exception("Invalid operator");
        }

        public dynamic Visit_Print(dynamic node)
        {
            var result = Visit(node.Expression);
            Console.WriteLine(result);
            return result;
        }

        public dynamic Visit_Concatenation(dynamic node)
        {
            return Visit(node.Right).ToString() + Visit(node.Left).ToString();
        }

        // Send the data to save a function on the SymbolTable
        public dynamic Visit_FunctionDeclaration(dynamic node)
        {
            List<string> parameters = new List<string>();

            foreach (var item in node.Parameters)
                parameters.Add(item.VarName);

            SymbolTable.AddFunction(node.Name, parameters, node.Expression);
            return node;
        }

        // Evaluate a function then have been declare
        public dynamic Visit_FunctionCall(dynamic node)
        {            
            List<object> parameters = new List<object>();

            foreach (var item in node.Parameters)
                parameters.Add(Visit(item));

            AST result = SymbolTable.CallFunction(node.FunctionName, parameters);

            var Value = Visit(result);
            SymbolTable.PopTable();
            return Value;
        }

        // Declare a function and return the evaluation of the body
        public dynamic Visit_LetIN(dynamic node)
        {
            SymbolTable.PushTable();

            foreach (AST item in node.VariablesDeclarations)
            {
                Visit(item);
            }
            
            var Value = Visit(node.InNode);
            SymbolTable.PopTable();
            return Value;
        }

        // Execute the if or else block en dependecy of the condition evaluation's
        public dynamic Visit_IfElse(dynamic node)
        {
            if (Visit(node.Condition))
                return Visit(node.IfBlock);
            else
                return Visit(node.ElseBlock);
        }

        // Assign the value of a variable
        public dynamic Visit_Assign(dynamic node)
        {
            string name = node.Variable.VarName;
            var value = Visit(node.Expression);

            if (value is double)
            {
                SymbolTable.AddSymbol(name, value, SymbolTable.VariableType.Double);
            }
            else if (value is string)
            {
                SymbolTable.AddSymbol(name, value, SymbolTable.VariableType.String);
            }
            else if (value is bool)
            {
                SymbolTable.AddSymbol(name, value, SymbolTable.VariableType.Bool);
            }

            return name;
        }

        // Get the value of a variable ob the simbol table
        public dynamic Visit_Var(dynamic node)
        {
            string name = node.VarName;
            return SymbolTable.GetSymbol(name).Item1;
        }

        // Evaluate the AST 
        public dynamic Interpret(string text)
        {
            Lexer lexer = new Lexer(text);
            Parser parser = new Parser(lexer);
            AST tree = parser.Parse();

            if (tree.GetType().Name == "FunctionCall")
            {
                var Value = Visit(tree);
                Console.WriteLine(Value);
                return Value;
            }
            else
                return Visit(tree);
        }
    }
}
