using TreeTraversal;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class Comparator : Operator
    {

        public Comparator(string operationValue)
        {
            this.value = operationValue;
        }

        public Comparator() // parameterless option
        {

        }
    }
}
