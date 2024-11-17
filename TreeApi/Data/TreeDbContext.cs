using Microsoft.EntityFrameworkCore;
using TreeApi.Models;

namespace TreeApi.Data
{
    public class TreeDbContext : DbContext
    {
        public DbSet<TreeNode> TreeNodes { get; set; }

        public TreeDbContext(DbContextOptions<TreeDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи между ParentId и дочерними элементами
            modelBuilder.Entity<TreeNode>()
                .HasMany(t => t.Children)
                .WithOne()
                .HasForeignKey(t => t.ParentId)
                .OnDelete(DeleteBehavior.Cascade); // Удаляем дочерние элементы при удалении родителя
        }

        public void SeedInitialData()
        {
            if (!TreeNodes.Any())
            {
                var category = new TreeNode { Name = "Категория" };
                TreeNodes.Add(category);
                SaveChanges(); // Сохраняем, чтобы получить Id

                var electronics = new TreeNode { Name = "Электроника", ParentId = category.Id };
                var clothing = new TreeNode { Name = "Одежда", ParentId = category.Id };

                TreeNodes.AddRange(electronics, clothing);
                SaveChanges();

                var mobiles = new TreeNode { Name = "Мобильные телефоны", ParentId = electronics.Id };
                var laptops = new TreeNode { Name = "Ноутбуки", ParentId = electronics.Id };
                var accessories = new TreeNode { Name = "Аксессуары", ParentId = electronics.Id };

                var menClothing = new TreeNode { Name = "Мужская одежда", ParentId = clothing.Id };
                var womenClothing = new TreeNode { Name = "Женская одежда", ParentId = clothing.Id };

                TreeNodes.AddRange(mobiles, laptops, accessories, menClothing, womenClothing);

                SaveChanges();
            }
        }

    }
}
