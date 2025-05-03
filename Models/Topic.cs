using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class Topic
    {
        [Key] //Đây là khóa chính bắt buộc
        public int TopicId { get; set; }

        [Required]
        [Display(Name = "Tên chủ đề")]
        public string Name { get; set; }

        public ICollection<ArticleTopic> ArticleTopics { get; set; }
    }
}
