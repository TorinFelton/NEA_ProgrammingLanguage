using Errors;
using Evaluator_Module.ExpressionEvaluation;
using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Evaluator_Module.ExpressionEvaluation.Conditions
{
    class ConditionTree
    {
        public static TreeNode BuildAST(List<Token> infix)
        /*
         * This is a reusage of the same algorithm used for maths expressions, but instead I've just treated boolean && and || as 'operators' and ! as an unary 'operator'
         * It is the same implementation apart from the objects used and a few small changes to adjust to boolean. Also, the RPN calculator 'Calculate' function does the comparisons instead of operations.
         */
        {
            Stack<Comparator> comparatorStack = new Stack<Comparator>();
            Stack<TreeNode> operandStack = new Stack<TreeNode>();

            foreach (Token token in infix)
            {

                if (token.Value().Equals("(")) comparatorStack.Push(new Comparator("("));
                // If it is the opening of a nested expression, just add it to the compstack - precedences values will be dealt with later


                else if (token.Type().Equals("bool")) 
                {
                    operandStack.Push(new BoolOperand(token.Value().ToLower().Equals("true") ? true : false));
                    // Simply create a new Operand (child class of TreeNode) node with the number in the character
                }


                else if (Comparator.precedences.ContainsKey(token.Value()) && !token.Value().Equals(")"))
                // BinOp.precedences is a DICTIONARY of all operators & their precedence (represented in Integers)
                // If token.Value() IS an OPERATOR and is NOT ")"
                {
                    // We have found an operator like +
                    // We need to resolve the operands into leaves first
                    while (comparatorStack.Count > 0 && Comparator.precedences[comparatorStack.Peek().value] >= Comparator.precedences[token.Value()])
                    // While the precedence of the top of the operatorStack is bigger than or equal to the precedence of the char
                    // Remember that precedences are stored as Integers, so we can compare them easily like this
                    {
                        Comparator comparator = comparatorStack.Pop();
                        // This will be the parent node of our 'leaf'
                        // the '+' in the example in the topmost comment

                        if (comparator.value.Equals("!")) // unary NOT
                        {
                            comparator.left = operandStack.Pop();
                        }

                        else
                        {
                            // Reversed as the second op was pushed at the end
                            comparator.right = operandStack.Pop();
                            comparator.left = operandStack.Pop();
                            // child nodes '1' and '2' added (following example in top comment)
                            // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                        }

                        operandStack.Push(comparator); // Leaf created! Now push parent node of leaf back onto numStack
                    }
                    // Now that our loop has iteratively created leaves and connected them for our tree, we have finished

                    // Now push operator at the end - we have not calculated anything with this one yet
                    comparatorStack.Push(new Comparator(token.Value()));
                }

                else if (token.Value().Equals(")")) // End of nested () expression
                {
                    while (comparatorStack.Count > 0 && !comparatorStack.Peek().value.Equals("("))
                    {
                        Comparator comparator = comparatorStack.Pop();

                        if (comparator.value.Equals("!")) // unary NOT
                        {
                            comparator.left = operandStack.Pop();
                        }

                        else
                        {
                            // Reversed as the second op was pushed at the end
                            comparator.right = operandStack.Pop();
                            comparator.left = operandStack.Pop();
                            // child nodes '1' and '2' added (following example in top comment)
                            // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                        }

                        operandStack.Push(comparator); // Leaf created! Now push parent node of leaf back onto numStack
                    }
                    // Similar loop to previously used - this time to resolve everything that we have collected inside the brackets.
                    // the minute we run into another nest, '(', we can leave it for later.

                    comparatorStack.Pop(); // We still have the '(' operator that started this expr in brackets left, let's get rid of it.
                }

                else
                {
                    throw new SyntaxError(); // Don't recognise what kind of Token is in our expression.
                }
            }

            while (comparatorStack.Count > 0) // Same leaf-making loop as before but with slightly different condition
            {
                Comparator comparator = comparatorStack.Pop();

                if (comparator.value.Equals("!")) // unary NOT
                {
                    comparator.left = operandStack.Pop();
                }

                else
                {
                    // Reversed as the second op was pushed at the end
                    comparator.right = operandStack.Pop();
                    comparator.left = operandStack.Pop();
                    // child nodes '1' and '2' added (following example in top comment)
                    // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                }

                operandStack.Push(comparator); // Leaf created! Now push parent node of leaf back onto numStack
            } // While there are still operators left, make leaves of the remaining with their operands until no more to make

            // At this point, the root node should be left at the top of the numStack (and the only thing in it)
            return operandStack.Pop();
            // we have NOT calculated anything - just built a tree of the expression and returned its root as a TreeNode.
        }
    }
}
