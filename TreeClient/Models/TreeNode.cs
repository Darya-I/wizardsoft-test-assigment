public class TreeNode
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentId { get; set; }
    public List<TreeNode> Children { get; set; } = new();
}
