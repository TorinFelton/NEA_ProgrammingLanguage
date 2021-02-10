using TreeTraversal;
using System;
using System.Collections.Generic;
using System.Data;
using Lexer_Module;
using Errors;

namespace Evaluator_Module.ExpressionEvaluation.Algorithms
{
    class TreeBuilder
    {
        public static TreeNode BuildAST(List<Token> infix)
        /*
         * 
         *
         * This is an implementation of Djikstra's Shunting-yard algorithm
         * It is not 100% true to the original, but quite close
         * 
         * We use two stacks: one for operators (e.g +, -, /) and one for the numbers (integers only supported)
         * The numstack is not actually storing Integer types, but TreeNodes that represent Integers
         *      The numstack TreeNodes could just be of value '1' or could ALSO be a 'leaf' of the tree
         *      A 'leaf' is a calculation that represents an Integer to be calculated
         *      An example leaf could be:
         *                  +
         *                1   2
         *      Programatically, this will just be given as the '+' root node with .Left and .Right being nodes '1' & '2'
         *
         *
         *
         * INPUT: List of Token objects that represent a mathematical expression, e.g ['1', '+', '2']
         * (just showing Token.Value() as list elements for demo)
         *
         * OUTPUT: The ROOT node of the Abstract Syntax Tree (AST) as a TreeNode object.
         * The parents of any nodes in this AST could be a Num or BinOp (both inherit from TreeNode)
         * Num: Represents just an Integer itself
         * BinOp: Represents an Operator. Left & Right nodes of a BinOp are to be the operands.
         */
        {
            Stack<BinOp> operatorStack = new Stack<BinOp>();
            Stack<TreeNode> numStack = new Stack<TreeNode>();

            infix = findUnaryMinus(infix); // Replace unary minus with "_" to distinguish

            foreach (Token token in infix)
            {

                if (token.Value().Equals("(")) operatorStack.Push(new BinOp("("));
                // If it is the opening of a nested expression, just add it to the opstack - precedences values will be dealt with later


                else if (token.Type().Equals("number")) // If it is a number (Integers only supported), add to numStack
                {
                    numStack.Push(
                        new Num(
                            int.Parse(token.Value()
                        )));
                    // Simply create a new Num (child class of TreeNode) node with the number in the character, converted to Integer type.
                }


                else if (BinOp.precedences.ContainsKey(token.Value()) && !token.Value().Equals(")"))
                    // BinOp.precedences is a DICTIONARY of all operators & their precedence (represented in Integers)
                    // If token.Value() IS an OPERATOR and is NOT ")"
                {
                    // We have found an operator, e.g '+'
                    // We need to resolve the operands into leaves of the tree first
                    while (operatorStack.Count > 0 && BinOp.precedences[operatorStack.Peek().value] >= BinOp.precedences[token.Value()])
                        // While the precedence of the top of the operatorStack is bigger than or equal to the precedence of the char
                        // Remember that precedences of operators are stored as Integers in the dictionary, so we can compare them easily like this
                    {
                        BinOp binOperator = operatorStack.Pop();
                        // This will be the parent node of our 'leaf'
                        // the '+' in the example in the topmost comment

                        if (binOperator.value.Equals("_")) // unary minus
                        {
                            binOperator.left = numStack.Pop(); // only needs one operand as unary minus only takes one
                        }

                        else
                        {
                            // Reversed as the second op was pushed at the end
                            binOperator.right = numStack.Pop();
                            binOperator.left = numStack.Pop();
                            // child nodes '1' and '2' added (following example in top comment)
                            // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                        }

                        numStack.Push(binOperator); // Leaf created! Now push parent node of leaf back onto numStack
                    }
                    // Now that our loop has iteratively created leaves and connected them for our tree, we have finished

                    // Now push operator at the end - we have not calculated anything with this one yet
                    operatorStack.Push(new BinOp(token.Value()));
                }

                else if (token.Value().Equals(")")) // End of nested () expression
                {
                    while (operatorStack.Count > 0 && !operatorStack.Peek().value.Equals("("))
                    {
                        BinOp binOperator = operatorStack.Pop();

                        if (binOperator.value.Equals("_")) // unary minus
                        {
                            binOperator.left = numStack.Pop();
                        }

                        else
                        {
                            // Reversed as the second op was pushed at the end
                            binOperator.right = numStack.Pop();
                            binOperator.left = numStack.Pop();
                            // child nodes '1' and '2' added (following example in top comment)
                            // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                        }

                        numStack.Push(binOperator);
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
                BinOp binOperator = operatorStack.Pop();

                if (binOperator.value.Equals("_")) // unary minus
                {
                    binOperator.left = numStack.Pop();
                }

                else
                {
                    // Reversed as the second op was pushed at the end
                    binOperator.right = numStack.Pop();
                    binOperator.left = numStack.Pop();
                    // child nodes '1' and '2' added (following example in top comment)
                    // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)
                }

                numStack.Push(binOperator);
            } // While there are still operators left, make leaves of the remaining with their operands until no more to make

            // At this point, the root node should be left at the top of the numStack (and the only thing in it)
            return numStack.Pop();
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
                        if (index == 0 || exprTokens[index - 1].Value().Equals("(") || exprTokens[index - 1].Type().Equals("operator"))
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
