using Lexer_Module;
using Parser_Module;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parser_Module.Events
{
    class FuncCall : Event
    {
        private string funcName;
        private List<Token> arguments; 
            // Do not be confused! Functions can NOT take multiple arguments
            // This 'arguments' refers to ANY tokens INSIDE the () brackets
            // e.g outputln(1 + 2 + 3);
            // arguments = ['1', '2', '3'] here (list of Tokens not just chars)
            // arguments is a list of Tokens representing an expression, like the 1+2+3 - the Evaluator will resolve this.

        public FuncCall(string funcName, List<Token> arguments)
        {
            this.type = "FUNC_CALL";
            this.funcName = funcName.ToLower();
            this.arguments = arguments;
        }

        public string Name() { return funcName; }

        public List<Token> Arguments() { return arguments; }

        public override string ToString()
        {
            List<string> argumentTokens = new List<string>();
            foreach (Token tok in arguments) argumentTokens.Add(tok.Value());

            return "FUNC_CALL: {name: '" + funcName + "', argument tokens: [" + String.Join(", ", argumentTokens) + "]}";
        }
    }
}
