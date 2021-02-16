using Errors;
using Evaluator_Module.ExpressionEvaluation;
using Lexer_Module;
using System;
using System.Collections.Generic;
using System.Text;
using TreeTraversal;

namespace Evaluator_Module.ExpressionEvaluation.Resolver
{
    class TreeBuilder
    {
        public static TreeNode BuildAST(List<Token> infix)
        /*
         * This is a reusage of the same algorithm used for maths expressions, but instead I've just treated boolean && and || as 'operators' and ! as an unary 'operator'
         * It is the same implementation apart from the objects used and a few small changes to adjust to boolean. Also, the RPN calculator 'Calculate' function does the comparisons instead of operations.
         */
        {
            Stack<Operator> operatorStack = new Stack<Operator>();
            Stack<TreeNode> operandStack = new Stack<TreeNode>();

            foreach (Token token in infix)
            {

                if (token.Value().Equals("(")) operatorStack.Push(new Operator("("));
                // If it is the opening of a nested expression, just add it to the compstack - precedences values will be dealt with later


                else if (token.Type().Equals("bool")) 
                {
                    operandStack.Push(new BoolOperand(token.Value().ToLower().Equals("true") ? true : false));
                    // Add boolean operand. Convert string -> bool type.
                }

                else if (token.Type().Equals("number"))
                {
                    operandStack.Push(new Number(int.Parse(token.Value())));
                    // add numeric operand. Convert string -> int type.
                }

                else if (token.Type().Equals("string"))
                {
                    operandStack.Push(new StringOperand(token.Value()));
                }


                else if (Operator.precedences.ContainsKey(token.Value()) && !token.Value().Equals(")"))
                // If precedences dictionary contains key token.Value() and it isn't ")"
                {
                    // We have found an operator like +
                    // We need to resolve the operands into leaves first

                    while (operatorStack.Count > 0 && Operator.precedences[operatorStack.Peek().value] >= Operator.precedences[token.Value()])
                    // While the precedence of the top of the operatorStack is bigger than or equal to the precedence of the char
                    // Remember that precedences are stored as Integers, so we can compare them easily like this
                    {
                        Operator oper = operatorStack.Pop();
                        // This will be the parent node of our 'leaf'
                        // the '+' in the example in the topmost comment

                        if (oper.value.Equals("!") || oper.value.Equals("_")) // unary NOT or unary MINUS
                        {
                            oper.left = operandStack.Pop();
                        }

                        else
                        {
                            // Reversed as the second op was pushed at the end
                            oper.right = operandStack.Pop();
                            oper.left = operandStack.Pop();
                            // child nodes '1' and '2' added (following example in top comment)
                            // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                        }

                        operandStack.Push(oper); // Leaf created! Now push parent node of leaf back onto numStack
                    }
                    // Now that our loop has iteratively created leaves and connected them for our tree, we have finished

                    // Now push operator at the end - we have not calculated anything with this one yet
                    operatorStack.Push(new Operator(token.Value()));
                }

                else if (token.Value().Equals(")")) // End of nested () expression
                {
                    while (operatorStack.Count > 0 && !operatorStack.Peek().value.Equals("("))
                    {
                        Operator oper = operatorStack.Pop();

                        if (oper.value.Equals("!") || oper.value.Equals("_")) // unary NOT or unary MINUS
                        {
                            oper.left = operandStack.Pop();
                        }

                        else
                        {
                            // Reversed as the second op was pushed at the end
                            oper.right = operandStack.Pop();
                            oper.left = operandStack.Pop();
                            // child nodes '1' and '2' added (following example in top comment)
                            // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                        }

                        operandStack.Push(oper); // Leaf created! Now push parent node of leaf back onto numStack
                    }
                    // Similar loop to previously used - this time to resolve everything that we have collected inside the brackets.
                    // the minute we run into another nest, '(', we can leave it for later.

                    operatorStack.Pop(); // We still have the '(' operator that started this expr in brackets left, let's get rid of it.
                }

                else
                {
                    throw new SyntaxError(); // Don't recognise what kind of Token is in our expression.
                }
            }

            while (operatorStack.Count > 0) // Same leaf-making loop as before but with slightly different condition
            {
                Operator oper = operatorStack.Pop();

                if (oper.value.Equals("!") || (oper.value.Equals("_"))) // unary NOT or unary MINUS
                {
                    oper.left = operandStack.Pop();
                }

                else
                {
                    // Reversed as the second op was pushed at the end
                    oper.right = operandStack.Pop();
                    oper.left = operandStack.Pop();
                    // child nodes '1' and '2' added (following example in top comment)
                    // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                }

                operandStack.Push(oper); // Leaf created! Now push parent node of leaf back onto numStack
            } // While there are still operators left, make leaves of the remaining with their operands until no more to make

            // At this point, the root node should be left at the top of the numStack (and the only thing in it)
            return operandStack.Pop();
            // we have NOT calculated anything - just built a tree of the expression and returned its root as a TreeNode.
        }

        public static List<Token> findUnaryMinus(List<Token> exprTokens)
        // This can probably be designed and optimised better but it was a quick implementation to prove concept
        {
            List<Token> toReturn = new List<Token>();

            int index = 0;

            while (index < exprTokens.Count)
            {
                Token tok = exprTokens[index]; // Tokens are stored like this: (type, value). Values are stored as strings for other reasons outside of expr evaluation.
                if (tok.Type().Equals("operator"))
                {
                    if (tok.Value().Equals("-"))
                    {
                        if (index == 0 || exprTokens[index - 1].Value().Equals("(") || exprTokens[index - 1].Type().Equals("operator") || exprTokens[index - 1].Type().Equals("grammar"))
                        // If the FIRST operator is -
                        // OR IF the Token before is '('
                        // OR IF the Token before is any other operator

                        // ^ these are the rules for recognising an UNARY minus sign
                        {
                            // unary as '-' at the beginnign of expr
                            toReturn.Add(new Token("operator", "_"));
                        }
                        else toReturn.Add(tok); // else just add the NORMAL minus sign
                    }
                    else toReturn.Add(tok);
                }
                else toReturn.Add(tok);
                index++;
            }

            return toReturn;
        }
    }
}
