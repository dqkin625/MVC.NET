using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Tài khoản")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string PasswordHash { get; set; }

        [Required]
        [Display(Name = "Vai trò")]
        public UserRole Role { get; set; }

        public ICollection<Article> Articles { get; set; } = new List<Article>();
    }
} 