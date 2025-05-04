using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class ArticleCreateViewModel
    {
        [Required]
        public Article Article { get; set; } = new Article();

        [Display(Name = "Chủ đề")]
        [Required(ErrorMessage = "Vui lòng chọn ít nhất một chủ đề.")] //đảm bảo người dùng chọn ít nhất 1 chủ đề
        public List<int> SelectedTopicIds { get; set; } = new List<int>(); //danh sách chủ đề đang chọn (Id)

        public List<SelectListItem> Topics { get; set; } = new List<SelectListItem>();  //danh sách chủ đề để hiển thị lên View
        //SelectListItem là một kiểu dữ liệu trong ASP.NET MVC dùng để tạo danh sách chọn (dropdown list, checkbox) trong các form HTML
    }
}
