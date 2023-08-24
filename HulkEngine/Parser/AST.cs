
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

    public class Print : AST
    {
        public Print(AST expression)
        {
            this.Expression = expression;
        }
        
        public AST Expression { get; set; }
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
