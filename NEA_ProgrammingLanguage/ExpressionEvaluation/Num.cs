using TreeTraversal;


namespace ExpressionEvaluation
{
    class Num : TreeNode
    {
        public int intValue { get; set; } // this.value still exists, but is in string form - it is useful to have a string value and int value instead of problematic ToInt conversions later

        public Num(int inputValue)
        {
            this.intValue = inputValue;
            this.value = inputValue.ToString(); // Both values equal to the same integer, just different types.
        }
    }
}
