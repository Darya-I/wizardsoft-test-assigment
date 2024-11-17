namespace TreeApi.Models
{
    public class TreeNode
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentId { get; set; } //null для корневых узлов
        public List<TreeNode> Children { get; set; } = new();
    }
}