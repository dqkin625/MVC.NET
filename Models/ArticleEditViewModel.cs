using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class ArticleEditViewModel
    {
        public Article Article { get; set; }

        [Display(Name = "Chủ đề")]
        public List<int> SelectedTopicIds { get; set; } = new List<int>();

        public List<SelectListItem> Topics { get; set; } = new List<SelectListItem>();
    }

}
