using Microsoft.EntityFrameworkCore;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class AppDbContext : DbContext //xác định các bảng, thiết lập quan hệ giữa các bảng, thiết lập khóa chính tổng hợp cho các bảng trung gian, tạo dữ liệu mẫu
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) //constructor cấu hình kết nối db thông qua Program.cs với options chứa chuỗi kết nối và các tùy chọn cấu hình DB
        {
        }

        // Mỗi DbSet tương ứng với một bảng trong cơ sở dữ liệu
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ArticleTopic> ArticleTopics { get; set; }
        //
        protected override void OnModelCreating(ModelBuilder modelBuilder) //cấu hình chi tiết
        {
            base.OnModelCreating(modelBuilder); //gọi đến phương thức OnModelCreating của lớp cha để thực hiện các cấu hình mặc định

            modelBuilder.Entity<ArticleTopic>() // Cấu hình quan hệ nhiều-nhiều giữa Article và Topic
                .HasKey(at => new { at.ArticleId, at.TopicId }); //2 cột ArticleId và TopicId là khóa chính

            modelBuilder.Entity<ArticleTopic>()
                .HasOne(at => at.Article)
                .WithMany(a => a.ArticleTopics)
                .HasForeignKey(at => at.ArticleId); //1 bài viết có nhiều chủ đề, Khóa ngoại ánh xạ tới Article.ArticleId

            modelBuilder.Entity<ArticleTopic>()
                .HasOne(at => at.Topic)
                .WithMany(t => t.ArticleTopics)
                .HasForeignKey(at => at.TopicId);

            // Cấu hình User
            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .IsRequired(); //Không được null

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
