namespace Lexer_Module
{
    class Token
    {
        private string type = null;
        private string value = null;
        public Token(string type, string value)
        {
            this.type = type; this.value = value;
        }

        public override string ToString()
        {
            return "('" + type + "', \"" + value + "\")"; // e.g (operation, "+")
        }

        public string Type()
        {
            return type;
        } // GetType() is a built-in C# method to get the type of variable, hence this is named Type() instead - no need to override.

        public string Value()
        {
            return value;
        }
    }
}
