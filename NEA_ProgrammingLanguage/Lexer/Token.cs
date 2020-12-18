namespace Lexer
{
    class Token
    {
        private string type = null;
        private string value = null;
        public Token(string typ, string val)
        {
            type = typ; value = val;
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
