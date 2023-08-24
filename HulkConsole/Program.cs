using System.Diagnostics.CodeAnalysis;
using HulkEngine;
string text = "";
Console.Write("calc>");
string? text2 = Console.ReadLine();


if (text2 != null)
    text = text2;

Lexer lexer = new(text);
Parser parser = new(lexer);
Interpreter interpreter = new(parser);
dynamic result = interpreter.Interpret();
Console.WriteLine("All Good");
