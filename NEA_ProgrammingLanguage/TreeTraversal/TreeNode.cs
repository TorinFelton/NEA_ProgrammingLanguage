namespace TreeTraversal
{
    abstract class TreeNode
    {
        // We are not going to regulate access to these as there are no conditions for them that we'd even be able to check
        // The type requirement is the only level of regulation we need for these attributes
        public TreeNode left = null;
        public TreeNode right = null;
        public string value = null;

        public TreeNode() { } // parameterless base for ease of use if you do not want to create a node with a left and right value

        public TreeNode(TreeNode inputLeft, TreeNode inputRight) // Simply exists for ease of use when creating a new TreeNode
        {
            this.left = inputLeft; this.right = inputRight;
        }
    }
}
