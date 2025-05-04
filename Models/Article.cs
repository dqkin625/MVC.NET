using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }

        [Required]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }

        //
        [Required]
        [Display(Name = "Tác giả")]
        public int AuthorId { get; set; } // Khóa ngoại ánh xạ tới User.UserID

        [ValidateNever] // Tránh validation để không bị lỗi khi bind User trong form (chỉ bind AuthorId)
        //VD: Gửi dữ liệu từ giao diện web về để lưu bài viết thì chỉ gửi mã số của tác giả chứ không phải toàn bộ thông tin như họ tên, email, ngày sinh...
        //Nếu không có ValidateNever thì hệ thống sẽ cố gắng bind toàn bộ thông tin của tác giả từ form về và các trường như tên, email,... sẽ null và lỗi thiếu thông tin cần thiết
        [ForeignKey("AuthorId")]
        public User Author { get; set; } // Navigation property truy vấn thống tin chi tiết của User
        //

        [Required]
        [Display(Name = "Ngày gửi")]
        [DataType(DataType.Date)]
        public DateTime SubmittedDate { get; set; }

        [Required]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }

        [Required]
        [Display(Name = "Nội dung")]
        public string Content { get; set; }


        //[Required]
        //[Display(Name = "Chủ đề")]
        //public int TopicId { get; set; }

        //[ForeignKey("TopicId")]
        //public Topic Topic { get; set; }
        [ValidateNever] //Không cần validate khi binding danh sách này từ form
        public ICollection<ArticleTopic> ArticleTopics { get; set; } // quan hệ nhiều nhiều với Topic thông qua ArticleTopic (bảng trung gian) - ánh xạ tới bảng Topic thông qua khóa ngoại TopicId trong ArticleTopic

        [Required]
        [Display(Name = "Trạng thái")]
        public ArticleStatus Status { get; set; }
    }
} 