using HulkEngine;
/*

string? text = Console.ReadLine();

if (text is not null)
{
    
    Lexer lexer = new Lexer(text);
    Parser parser = new Parser(lexer);
    Interpreter interpreter = new Interpreter(parser);
    var result = interpreter.Interpret();
}

*/

SymbolTable symbolTable = new SymbolTable();
Interpreter interpreter = new Interpreter(symbolTable);

Console.WriteLine("Bienvenidos al intérprete de HULK");

while(true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.Write(">> ");
    string? input = Console.ReadLine();

    if (input is not null)
    {
        if (input == "salir")
        {
            Console.WriteLine("Saliendo...");
            break;
        }

//        try
//        {
            Console.ForegroundColor = ConsoleColor.Green;
            var result = ProcessInput(input, interpreter);
//        }
//        catch (Exception ex)
//        {
//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine(ex.Message);
//            continue;
//        }
    }
    else break;
}

static object ProcessInput(string input, Interpreter interpreter)
{
    return interpreter.Interpret(input);
}
