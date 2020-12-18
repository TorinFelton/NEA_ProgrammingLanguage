namespace ExpressionEvaluation.Algorithms
{
    class ExpressionToken
    {
        public string value = null;
        public bool isNumber = false;

        public ExpressionToken(string inputValue, bool isNumber)
        {
            this.isNumber = isNumber;
            this.value = inputValue;
        }

    }
}
