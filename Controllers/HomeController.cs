using Microsoft.AspNetCore.Mvc;
using QuanLyBaiBaoKhoaHoc.Models;

namespace QuanLyBaiBaoKhoaHoc.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserId") == null) //kiểm tra người dùng đã đăng nhập chưa bằng các kiểm tra Session
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
