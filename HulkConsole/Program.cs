using HulkEngine;


string? text = Console.ReadLine();

if (text is not null)
{
    
    Lexer lexer = new(text);
    Parser parser = new(lexer);
    Interpreter interpreter = new(parser);
    var result = interpreter.Interpret();
}

/*
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

        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var result = ProcessInput(input);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            continue;
        }
    }
    else break;
}

static object ProcessInput(string input)
{
    Lexer lexer = new(input);
    Parser parser = new(lexer);
    Interpreter interpreter = new(parser);
    return interpreter.Interpret();
}
*/