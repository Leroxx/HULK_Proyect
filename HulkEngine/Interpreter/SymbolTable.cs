
namespace HulkEngine
{
    public class SymbolTable : NodeVisitor
    {
        public enum VariableType
        {
            Double,
            String,
            Bool
        }

        // Stack to store variable declarations in each environment
        private Stack<Dictionary<string, Tuple<object, VariableType>>> SymbolStack;

        // to store function declarations
        private Dictionary<string, Tuple<List<string>, AST>> FunctionsList;

        public SymbolTable()
        {
            SymbolStack = new Stack<Dictionary<string, Tuple<object, VariableType>>>();
            FunctionsList =  new Dictionary<string, Tuple<List<string>, AST>>();
        }

        // Create a new environment
        public void PushTable()
        {
            SymbolStack.Push(new Dictionary<string, Tuple<object, VariableType>>());
        }

        // Deletes an environment
        public Dictionary<string, Tuple<object, VariableType>> PopTable()
        {
            if (SymbolStack.Count > 0)
            {
                return SymbolStack.Pop();
            }
            else
                throw new Exception ("Error");
        }

        // If an environment has been created, then add the variable declaration to the stack.
        public void AddSymbol(string name, object value, VariableType type)
        {
            if (SymbolStack.Count > 0)
            {
                SymbolStack.Peek()[name] = Tuple.Create(value, type);
            }
            else
                throw new Exception ("Error");
        }

        // Add a new function to the dictionary. Each function will have a list of parameters, 
        // corresponding to the variable names that will be created and an expression to evaluate.
        public void AddFunction(string name, List<string> parameters, AST expression)
        {
            FunctionsList[name] = Tuple.Create(parameters, expression);
        }

        // Gets a declared variable starting with the highest priority environment.
        public Tuple<object, VariableType> GetSymbol(string name)
        {
            foreach (var table in SymbolStack)
            {
                if (table.ContainsKey(name))
                    return table[name];
            }

            throw new ArgumentException("'" + name + "' has not been declared");
        }

        // Returns the expression of a corresponding function and creates the environment for the arguments to be declared.
        public dynamic CallFunction(string name, List<object> parameters)
        {
            List<string> names = FunctionsList[name].Item1;
            AST expression = FunctionsList[name].Item2;

            // If the number of arguments does not match the number of arguments received by the function, the execution is terminated.
            if (names.Count != parameters.Count)
                throw new ArgumentException("function' " + name + "' recive " + names.Count + " arguments");

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
