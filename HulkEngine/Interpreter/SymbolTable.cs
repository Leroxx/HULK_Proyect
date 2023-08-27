using System;

namespace HulkEngine
{
    public class SymbolTable
    {
        public enum VariableType
        {
            Double,
            String
        }

        private Stack<Dictionary<string, Tuple<object, VariableType>>> SymbolStack;

        public SymbolTable()
        {
            SymbolStack = new Stack<Dictionary<string, Tuple<object, VariableType>>>();
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

        public Tuple<object, VariableType> GetSymbol(string name)
        {
            foreach (var table in SymbolStack)
            {
                if (table.ContainsKey(name))
                    return table[name];
            }
            throw new Exception ("Error");
        }
    }
}