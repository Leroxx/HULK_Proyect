# HULK_Proyect     Richard Avila Entenza C111
> Proyecto de Programación II. Facultad de Matemática y Computación. Universidad de La Habana. Curso 2023.

HULK_Proyect es una aplicación de consola desarrollada con tecnología .Net 7.0 utilizando C# como lenguaje de programación. Dicha aplicación es capaz de interpretar un subconjunto de las funcionalidades del lenguaje de programacón HULK de la facultad de Ciencias de la Computación de la Universidad de la Habana.

### Inicializando el proyecto
>>    dotnet run --project HulkConsole
 

La aplicación esta divida en dos proyectos fundamentales:
- `HulkConsole´ aplicación de consola encargada de la parte gráfica del proyecto y encargada de servir los datos de las entradas del usuario.
- `HUlkEngine´ encargada de interpretar y enviar los resultados de las entradas de los usuarios.

## HulkConsole
Como se mecionaba anteriormente HulkConsole solo se encarga de la parte gráfica de nuestro proyecto por lo cual no contiene mucho contenido a explicación. Solamente resaltar de la misma que las entradas de los usuarios se mostraran de color azul y los resultados de la misma verdes si han sido satisfactorios y rojo en caso de haberse cometido un error en su escritura o utilización.

## HulkEngine
HulkEngine esta divido en tres componentes fundamentales `Lexer´, `Parser´ e `Interpreter´.

### Lexer
El `Lexer´ es el encargado del proceso de Tokenización de obtiene el código enviado por el usuario, lo cual seria una cadena de texto y en el Lexer este es procesado de manera lineal, caracter a caracter hasta obtener un ´Token´ válido para el lenguaje y ser enviado al ´Parser´ en caso de no ser un ´Token´ válido en el lenguaje entonces se envia un excepción donde resaltando en que parte del la cadena ha ocurrido el error Léxico.

Contiene las bibliotecas de clases ´Token´ donde estan declarados todos los token válidos del lenguaje y ´Lexer´ donde se realiza el proceso de Tokenización.

### Parser
Una vez obtenido el primer ´Token´ utilizando Parsing Recursivo Descendente se procesando todos los Token hasta llegar al Token de final de línea del ´;', al culmino del procesp de parsing se obtiene un ´Abstract Syntax Tree (AST)´ el cual será interpretado. Si durante el proceso de parsing se esperaba un Token en lugar de otro, o se omiten palabras reservadas del lenguaje o cualquier otro error sintáctico se termina el proceso de parsing y se envia un excepción descrbiendo el error cometido.

Contiene las bibliotecas de clases ´AST´ que contiene la declaracion de las clases de los nodos de HULK y ´Parser´ donde se realiza la construcción de los mismos quedando asi el AST que constituiría el programa a evaluar por el intérprete.

### Interpreter
El 'Interpreter' es el encargado de evaluar el AST obtenido del Parser, una vez se ha creado este se recorre el arbol Top-Down hasta obtener el valor de la raíz del árbol, si durante las visita a los nodos ocurre algun error semántico ya sea con tipos de datos, argumentos de funciones o mal uso de operadores se envía un excepción describiendo el error cometido.

El intérprete consta de tres bibliotecas de clases:
´NodeVisitor´ se encarga de invocar el metodo de visita al node correspondiente, siempre que se reliza la visita de un nodo se llama el método ´Visit´ que recibe un nodo de tipo ´dynamic´ de ´NodeVisitor´, segun el tipo de nodo que ha recibo el metodo se genera la visita correspondiente a dicho nodo. Por ejemplo si se envía un nodo de tipo ´BinOP´ se genera la visita ´Visit_BinOP´.

´SymbolTable´ tabla de símbolos encarga de guarda los valares y las expresiones de las variables y las funciones respectivamente para luego ser utilizadas, tambien se manejan los ambitos de declaraciones de la variables y los tipos de datos de las mismas.

´Interpreter´ biblioteca donde se implementan las visitas correspondientes a cada nodo y contiene el método ´Interpret´ encargado de evaluar el AST.

### Resumen
Para mas detalles acerca de la implementación del proyecto en el informe del mismo se presentan mas aspectos.
