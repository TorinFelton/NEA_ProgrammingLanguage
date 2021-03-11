using DataStructures;
using Interfaces;
using Lexer_Module;
using Parser_Module;
using System;


namespace ExamplePlugin
{
    public class ExamplePlugin : IPlugin
    {
        public string Name { get => "TestPlugin1"; }

    }

    public class TestStatement : Step
    {
        public override string ToString() => "null";
        
        public TestStatement()
        {
            this.type = "TestStatement";
        }

        public string Type() => this.type;

    }

    public class ExampleParseHandler : IParseHandler
    {
        public Step Result(ref TokenQueue tokQ, Token currentTok)
        {
            return new TestStatement();
        }
    }
}
