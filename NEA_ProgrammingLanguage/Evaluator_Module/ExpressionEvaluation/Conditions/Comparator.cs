using TreeTraversal;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation.Conditions
{
    class Comparator : TreeNode
    {
        public static Dictionary<string, int> precedences = new Dictionary<string, int>()
        {
            {"!", 3 },
            {"||", 2 },
            {"&&", 2 },
            {")", 2 },
            {"(", 1 }
        }; // Static dictionary of precedence levels represented by ints

        public Comparator(string operationValue)
        {
            this.value = operationValue;
        }

        public Comparator() // parameterless option
        {

        }
    }
}
