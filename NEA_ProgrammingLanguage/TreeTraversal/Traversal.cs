using System.Collections.Generic;

namespace TreeTraversal
{
    class Traversal
    {
        /*
         * All three of these are recursive - they create their own lists which are returned and added onto the list of the parent.
         * The only one *actually* used in our program is postOrder for the postfix expressions, but the other two are 
         * useful for testing the Abstract Syntax Trees are correctly being made.
         * 
         * Using inOrder traversal on an AST will output the expression in the original, human-readable mathematical form.
         */
        public static List<TreeNode> postOrder(TreeNode node)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            if (node.left != null) nodes.AddRange(postOrder(node.left)); // Add recursive call onto the end (via AddRange) of the List
            if (node.right != null) nodes.AddRange(postOrder(node.right));
            nodes.Add(node); // As it's post order, add the parent node to the end.
            return nodes;
        }

        public static List<TreeNode> inOrder(TreeNode node)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            if (node.left != null) nodes.AddRange(inOrder(node.left));
            nodes.Add(node); // In order so add parent node in between.
            if (node.right != null) nodes.AddRange(inOrder(node.right));
            return nodes;
        }

        public static List<TreeNode> preOrder(TreeNode node)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            nodes.Add(node); // Pre order so add parent node first.
            if (node.left != null) nodes.AddRange(preOrder(node.left));
            if (node.right != null) nodes.AddRange(preOrder(node.right));
            return nodes;
        }
    }
}
