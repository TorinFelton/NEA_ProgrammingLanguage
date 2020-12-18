namespace ExpressionEvaluation.TreeTraversal
{
    abstract class TreeNode
    {
        public TreeNode left = null;
        public TreeNode right = null;
        public string value = null;

        public TreeNode() // parameterless base for ease of use if you do not want to create a node with a left and right value
        {

        }

        public TreeNode(TreeNode inputLeft, TreeNode inputRight) // Simply exists for ease of use when creating a new TreeNode
        {
            this.left = inputLeft; this.right = inputRight;
        }
    }
}
