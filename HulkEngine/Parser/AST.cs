using System.Globalization;

namespace HulkEngine
{
    public class AST { }

    public class BinOP : AST
    {
        public BinOP(AST left, Token op, AST right)
        {
            this.Left = left;
            this.OP = op;
            this.Right = right;
        }

        public AST Left { get; set; }
        public AST Right { get; set; }
        public Token OP { get; set; }
    }

    public class Num : AST
    {
        public Num(Token token)
        {
            this.TokenType = token.Type;
            this.Value = double.Parse(token.Value, CultureInfo.InvariantCulture);
        }

        public Token.TokenType TokenType { get; set; }
        public double Value { get; set; }
    }

    public class String : AST
    {
        public String(Token token)
        {
            this.Value = token.Value;
        }

        public string Value { get; set; }
    }

    public class UnaryOP : AST
    {
        public UnaryOP(Token op, AST exp)
        {
            this.TokenType = op.Type;
            this.Exp = exp;
        }

        public Token.TokenType TokenType { get; set; }
        public AST Exp { get; set; }
    }

    public class MathFunction : AST
    {
        public MathFunction(Token function, AST expression)
        {
            this.Function = function;
            this.Expression = expression;
        }

        public Token Function { get; set; }
        public AST Expression { get; set; }
    }

    public class LogFunction : AST
    {
        public LogFunction(AST expression_base, AST expression)
        {
            this.Expression_Base = expression_base;
            this.Expression = expression;
        }

        public AST Expression_Base { get; set; }
        public AST Expression { get; set; }
    }

    public class Pow : AST
    {
        public Pow(AST expression, AST exp)
        {
            this.Expression = expression;
            this.Exp = exp;
        }

        public AST Expression { get; set; }
        public AST Exp { get; set; }
    }

    public class Constants : AST
    {
        public Constants(Token token)
        {
            this.Token = token;
        }

        public Token Token { get; set; }
    }

    public class Concatenation : AST
    {
        public Concatenation(AST right, AST left)
        {
            this.Right = right;
            this.Left = left;
        }

        public AST Right { get; set; }
        public AST Left {get; set; }
    }

    public class Print : AST
    {
        public Print(AST expression)
        {
            this.Expression = expression;
        }
        
        public AST Expression { get; set; }
    }

    public class FunctionDeclaration : AST
    {
        public FunctionDeclaration(string name, LinkedList<AST> parameters, AST expression)
        {
            this.Name = name;
            this.Parameters = parameters;
            this.Expression = expression;
        }

        public string Name { get; set; }
        public LinkedList<AST> Parameters { get; set; }
        public AST Expression { get; set; }
    }

    public class FunctionCall : AST
    {
        public FunctionCall(string name, List<AST> parameters)
        {
            this.FunctionName = name;
            this.Parameters = parameters;
        }

        public string FunctionName { get; set; }
        public List<AST> Parameters { get; set; }
    }

    public class LetIN : AST
    {
        public LetIN(LinkedList<AST> variablesDeclarations, AST inNode)
        {
            this.InNode = inNode;
            this.VariablesDeclarations = variablesDeclarations;
        }

        public AST InNode { get; set; }
        public LinkedList<AST> VariablesDeclarations { get; set; }
    }

    public class Assign : AST
    {
        public Assign(AST var, Token op, AST expression)
        {
            this.Variable = var;
            this.Token = op;
            this.Expression = expression;
        }

        public AST Variable { get; set; }
        public Token Token { get; set; }
        public AST Expression { get; set; }
    }

    public class Var : AST
    {
        public Var(Token token)
        {
            this.VarName = token.Value;
        }

        public string VarName { get; set; }
    }
}
