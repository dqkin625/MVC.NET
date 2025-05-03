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

        [Required]
        [Display(Name = "Tác giả")]
        public int AuthorId { get; set; }

        [ValidateNever] // Prevent validation for Author navigation property
        [ForeignKey("AuthorId")]
        public User Author { get; set; }

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
        [ValidateNever]
        public ICollection<ArticleTopic> ArticleTopics { get; set; }
        // quan hệ nhiều nhiều

        [Required]
        [Display(Name = "Trạng thái")]
        public ArticleStatus Status { get; set; }
    }
} 