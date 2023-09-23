using System.Reflection;
using HulkEngine;

SymbolTable symbolTable = new SymbolTable();
Interpreter interpreter = new Interpreter(symbolTable);

Console.WriteLine("Welcome to HULK's interpreter");

while (true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(">> ");
    string? input = Console.ReadLine();

    if (input is not null)
    {
        if (input == "exit")
        {
            Console.WriteLine("Exiting...");
            break;
        }

        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var result = ProcessInput(input, interpreter);
        }
        catch (Exception ex)
        {
            if (ex is TargetInvocationException)
            {
                Exception? original = ex;

                while (original is TargetInvocationException targetInvocationException)
                {
                    original = targetInvocationException.InnerException;
                }

                if (original is not null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("Semantic Error: " + original.Message);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
            }
            continue;
        }

    }
    else break;
}

static object ProcessInput(string input, Interpreter interpreter)
{
    return interpreter.Interpret(input);
}
