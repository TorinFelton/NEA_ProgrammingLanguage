using TreeTraversal;
using System.Collections.Generic;


namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class MathsOperator : Operator
    {
        public MathsOperator(string operationValue)
        {
            this.value = operationValue;
        }

        public MathsOperator() // parameterless option
        {

        }
    }
}
