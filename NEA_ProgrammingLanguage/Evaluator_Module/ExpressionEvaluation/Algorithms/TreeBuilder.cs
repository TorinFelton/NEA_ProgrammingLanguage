using TreeTraversal;
using System;
using System.Collections.Generic;
using System.Data;
using Lexer_Module;
using Errors;

namespace Evaluator_Module.ExpressionEvaluation.Algorithms
{
    class Postfix
    {
        public static TreeNode BuildAST(List<Token> infix)
        /*
         * It is easier to think of this as a reversal of an RPN calculation algorithm using stacks.
         * 
         * This is an implementation of Djikstra's Shunting-yard algorithm
         * It is not 100% true to the original; instead of resolving each expression in the stack it builds a tree
         * We use two stacks: one for operators (e.g +, -, /) and one for the operands (integer)
         * The numstack is not actually storing Integers, but more TreeNodes that represent Integers
         *      The numstack TreeNodes could just be of value '1' or could ALSO be a 'leaf' of the tree
         *      A 'leaf' is a small calculation that represents an Integer to be calculated
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

            foreach (Token token in infix)
            {
                if (token.Value().Equals("(")) operatorStack.Push(new BinOp("("));
                // If it is the opening of a nested expression, just add it to the opstack - precedences values will be dealt with later
                

                else if (token.Type().Equals("number")) // If it is a number (Integers only supported), add to numStack
                {
                    numStack.Push(new Num(int.Parse(token.Value())));
                    // Simply create a new Num (child class of TreeNode) node with the number in the character, converted to Integer type.
                }


                else if (BinOp.precedences.ContainsKey(token.Value()) && !token.Value().Equals(")"))
                    // BinOp.precedences is a DICTIONARY of all operators & their precedence (represented in Integers)
                    // If token.Value() IS an OPERATOR and is NOT ")"
                {
                    // We have found an operator like +
                    // We need to resolve the operands into leaves first
                    while (operatorStack.Count > 0 && BinOp.precedences[operatorStack.Peek().value] >= BinOp.precedences[token.Value()])
                        // While the precedence of the top of the operatorStack is bigger than or equal to the precedence of the char
                        // Remember that precedences are stored as Integers, so we can compare them easily like this
                    {
                        BinOp binOperator = operatorStack.Pop();
                        // This will be the parent node of our 'leaf'
                        // the '+' in the example in the topmost comment

                        // Reversed as the second op was pushed at the end
                        binOperator.right = numStack.Pop();
                        binOperator.left = numStack.Pop();
                        // child nodes '1' and '2' added (following example in top comment)
                        // The numstack does not just contain raw Integer nodes, it could have another leaf (a leaf resolves to an Integer)

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
                        // Reversed as the second op was pushed at the end
                        binOperator.right = numStack.Pop();
                        binOperator.left = numStack.Pop();

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
                // Reversed as the second op was pushed at the end
                binOperator.right = numStack.Pop();
                binOperator.left = numStack.Pop();

                numStack.Push(binOperator);
            } // While there are still operators left, make leaves of the remaining with their operands until no more to make

            // At this point, the root node should be left at the top of the numStack (and the only thing in it)
            return numStack.Pop();
            // we have NOT calculated anything - just built a tree of the expression and returned its root as a TreeNode.
        }

    }
}
