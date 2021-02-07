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
        // Arguments could be an expression or a single element referencing a variable name.

        public FuncCall(string funcName, List<Token> arguments)
        {
            this.type = "FUNC_CALL";
            this.funcName = funcName.ToLower();
            this.arguments = arguments;
        }

        public string GetName() { return funcName; }

        public List<Token> GetArguments() { return arguments; }

        public override string ToString()
        {
            List<string> argumentTokens = new List<string>();
            foreach (Token tok in arguments) argumentTokens.Add(tok.Value());

            return "FUNC_CALL: {name: '" + funcName + "', argument tokens: [" + String.Join(", ", argumentTokens) + "]}";
            // Return function name and argument (in form of expression represented by Tokens)
        }
    }

    class ReturnValue : Event
    {
        private List<Token> toReturn;

        public ReturnValue(List<Token> toReturn)
        {
            this.type = "RETURN_VALUE";
            this.toReturn = toReturn;
        }

        public List<Token> GetReturnedValue() { return toReturn; }


        public override string ToString()
        {
            string toOutput = "";
            foreach (Token tok in toReturn) toOutput += tok.Value();
            return "RETURN_VALUE: " + toOutput;
        }
    }
}
