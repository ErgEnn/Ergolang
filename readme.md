# Ergolang
An interpreted dynamic typed programming language with C# backend. Written based on book [Crafting Interpreters](https://craftinginterpreters.com).

This initial version of language is based on tree-walk interpreter so it is fairly slow. I am already reading the second part of the book where bytecode virtual machine is built for that language, but my coding is not that far yet.


## GenerateAst
In book, the writer wrote a tool to generate some classes automatically. Since C# has support for source generators, the same functionality is achieved with [Generator proj](/Ergolang/Generator/Generator.cs) and [template files](/Ergolang/Ergolang/Expr.template).