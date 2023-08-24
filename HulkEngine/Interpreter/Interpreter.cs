
namespace HulkEngine
{
    public class Interpreter : NodeVisitor
    {
        public Dictionary<string, object> Global_Scope = new Dictionary<string, object>();
        public Interpreter(Parser parser)
        {
            this.Parser = parser;
        }

        private Parser Parser { get; set; }
        
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

            throw new Exception("Invalid operator");
        }

        public dynamic Visit_Num(dynamic node)
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

        public dynamic Visit_LetIN(dynamic node)
        {
            foreach (AST item in node.VariablesDeclarations)
            {
                Visit(item);
            }

            return Visit(node.InNode);
        }

        public dynamic Visit_Assign(dynamic node)
        {
            string name = node.Variable.VarName;
            Global_Scope.Add(name, Visit(node.Expression));
            return name;
        }

        public dynamic Visit_Var(dynamic node)
        {
            string name = node.VarName;

            if (Global_Scope.ContainsKey(name))
            {
                return Global_Scope[name];
            }
            else
                throw new Exception("Variable no declarada");
        }

        public dynamic Interpret()
        {
            AST tree = Parser.Parse();
            return Visit(tree);
        }
    }
}