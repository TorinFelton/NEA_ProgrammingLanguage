using System;
using System.Collections.Generic;
using TreeTraversal;

namespace Evaluator_Module.ExpressionEvaluation.Conditions
{
    class RPN
    {
        public static bool Evaluate(List<TreeNode> nodes) // Input is a list of TreeNodes given in POSTFIX traversal order of the tree
        {
            Stack<TreeNode> nodeStack = new Stack<TreeNode>(); // Create stack to use for RPN, type TreeNode as it could be a BinOp or Num

            foreach (TreeNode treeNode in nodes)
            {
                if (treeNode is BoolOperand) nodeStack.Push(treeNode); // If it is a Num (Integer) then just push onto stack
                else if (treeNode is Comparator) // If it is a BinOp (operator, root of leaf) then pop last two operands and calculate:
                {
                    if (treeNode.value.Equals("!"))
                    {
                        BoolOperand arg = (BoolOperand)nodeStack.Pop();
                        nodeStack.Push(new BoolOperand(!arg.boolValue)); // unary minus, so negate the operand
                    }
                    else
                    {
                        // Pop last two
                        BoolOperand arg2 = (BoolOperand)nodeStack.Pop();
                        BoolOperand arg1 = (BoolOperand)nodeStack.Pop(); // Note they are reversed, the first one to be popped is the second argument in the expression.

                        nodeStack.Push(new BoolOperand(Calculate(arg1.boolValue, arg2.boolValue, treeNode.value))); // Create new Num (Integer) with result of calc
                    }
                }
            }
            BoolOperand result = (BoolOperand)nodeStack.Pop(); // Stack should be left with just one Num (Integer) as the final result
            return result.boolValue;
        }

        public static bool Calculate(bool arg1, bool arg2, string comparison) // Simplest way to 'act out' operators that are in string-form
        {
            bool result = false; // Default is false
            switch (comparison)
            {
                case "&&":
                    result = arg1 && arg2;
                    break;
                case "||":
                    result = arg1 || arg2;
                    break;
                default:
                    break; // do nothing as result = 0 already
            }
            return result;
        }
    }
}
