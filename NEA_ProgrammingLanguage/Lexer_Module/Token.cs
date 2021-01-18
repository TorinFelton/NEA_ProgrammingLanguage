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
            return "('" + type + "', \"" + value + "\")"; // e.g ('grammar', "=")
        }

        public string Type() 
            // It is easier to name getters as simply Type() or Value() instead of GetType() or GetValue() 
            // as these are built-in methods that shouldn't really be overriden
        {
            return type;
        }

        public string Value()
        {
            return value;
        }
    }
}
