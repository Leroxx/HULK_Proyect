using System.Linq.Expressions;
using System;
using System.Runtime.CompilerServices;
using System.Formats.Asn1;

namespace HulkEngine
{
    public class SymbolTable : NodeVisitor
    {
        public enum VariableType
        {
            Double,
            String
        }

        private Stack<Dictionary<string, Tuple<object, VariableType>>> SymbolStack;
        private Dictionary<string, Tuple<List<string>, AST>> FunctionsList;

        public SymbolTable()
        {
            SymbolStack = new Stack<Dictionary<string, Tuple<object, VariableType>>>();
            FunctionsList =  new Dictionary<string, Tuple<List<string>, AST>>();
        }

        public void PushTable()
        {
            SymbolStack.Push(new Dictionary<string, Tuple<object, VariableType>>());
        }

        public Dictionary<string, Tuple<object, VariableType>> PopTable()
        {
            if (SymbolStack.Count > 0)
            {
                return SymbolStack.Pop();
            }
            else
                throw new Exception ("Error");
        }

        public void AddSymbol(string name, object value, VariableType type)
        {
            if (SymbolStack.Count > 0)
            {
                SymbolStack.Peek()[name] = Tuple.Create(value, type);
            }
            else
                throw new Exception ("Error");
        }

        public void AddFunction(string name, List<string> parameters, AST expression)
        {
            FunctionsList[name] = Tuple.Create(parameters, expression);
        }

        public Tuple<object, VariableType> GetSymbol(string name)
        {
            foreach (var table in SymbolStack)
            {
                if (table.ContainsKey(name))
                    return table[name];
            }
            Console.WriteLine(name);
            throw new Exception ("Error");
        }

        public dynamic CallFunction(string name, List<object> parameters)
        {
            List<string> names = FunctionsList[name].Item1;
            AST expression = FunctionsList[name].Item2;

            PushTable();
                
            for (int i = 0; i < names.Count; i++)
            {
                if (parameters[i] is double)
                {
                    AddSymbol(names[i], parameters[i], SymbolTable.VariableType.Double);
                }
                else if (parameters[i] is string)
                {
                    AddSymbol(names[i], parameters[i], SymbolTable.VariableType.String);
                }
            }

            return expression;
        }
    }
}
