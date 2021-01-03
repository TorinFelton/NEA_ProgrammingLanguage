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
        }

        public string Value()
        {
            return value;
        }
    }
}
