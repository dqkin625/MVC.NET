using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class ArticleEditViewModel
    {
        public Article Article { get; set; } //khi load lên form, hệ thống sẽ tự động bind dữ liệu từ database vào Article

        [Display(Name = "Chủ đề")]
        public List<int> SelectedTopicIds { get; set; } = new List<int>(); //hiên thị danh sách chủ đề đã chọn (Id) trong form

        public List<SelectListItem> Topics { get; set; } = new List<SelectListItem>(); //danh sách chủ đề để hiển thị lên View
    }

}
