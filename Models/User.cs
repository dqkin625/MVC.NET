using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class User
    {
        [Key] //Khóa chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Tự động tăng (auto-increment) nếu không có thì EF sẽ không tự động tăng (Khóa chính và int hoặc long)
        public int UserId { get; set; }

        [Required] //Bắt buojc có giá trị (khác null)
        [Display(Name = "Họ tên")] //Gán tên hiển thị trên form là "Họ tên"
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)] //Gợi ý chỉ cần nhập ngày (không bao gồm giờ)
        public DateTime DateOfBirth { get; set; } //Kiểu dữ liệu thời gian

        [Required]
        [EmailAddress] //kiểm tra định dạng email hợp lệ
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Tài khoản")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)] //Gợi ý trình duyệt hiển thị trường này là ô mật khẩu
        [Display(Name = "Mật khẩu")]
        public string PasswordHash { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        public UserRole Role { get; set; } //UserRole là enum đã định nghĩa ở trên

        public ICollection<Article> Articles { get; set; } = new List<Article>(); //Thiết lập quan hệ 1-n với Article và ICollection là navigation property, hỗ trợ truy vấn thông qua EF Core
    }
} 