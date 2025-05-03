using Microsoft.EntityFrameworkCore;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ArticleTopic> ArticleTopics { get; set; } // ✅ thêm dòng này

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ nhiều-nhiều giữa Article và Topic
            modelBuilder.Entity<ArticleTopic>()
                .HasKey(at => new { at.ArticleId, at.TopicId }); // ✅ composite key

            modelBuilder.Entity<ArticleTopic>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTopics)
                .HasForeignKey(at => at.ArticleId);

            modelBuilder.Entity<ArticleTopic>()
                .HasOne(at => at.Topic)
                .WithMany(t => t.ArticleTopics)
                .HasForeignKey(at => at.TopicId);

            // Cấu hình User
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    FullName = "Admin",
                    DateOfBirth = new DateTime(2005, 6, 2),
                    Email = "doquockien12345@gmail.com",
                    Username = "admin",
                    PasswordHash = "admin",
                    Role = UserRole.Admin,
                }
            );
        }
    }
}
