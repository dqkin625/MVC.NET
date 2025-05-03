using Microsoft.AspNetCore.Mvc;
using QuanLyBaiBaoKhoaHoc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace QuanLyBaiBaoKhoaHoc.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // Hiển thị trang đăng nhập
        public IActionResult Login()
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password);

            if (user != null)
            {
                // Lưu thông tin vào session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role.ToString());

                TempData["Success"] = "Đăng nhập thành công!";
                return RedirectToAction("Index", "Home");
            }

            TempData["Error"] = "Tài khoản hoặc mật khẩu không đúng.";
            return View();
        }

        // Hiển thị trang đăng ký
        public IActionResult Register()
        {
            return View();
        }

        // Xử lý đăng ký
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Dữ liệu nhập chưa hợp lệ.";
                return View(user);
            }

            if (_context.Users.Any(u => u.Username == user.Username || u.Email == user.Email))
            {
                ModelState.AddModelError(string.Empty, "Tên tài khoản hoặc Email đã tồn tại.");
                return View(user);
            }

            try
            {
                user.Role = UserRole.Author;
                _context.Users.Add(user);
                _context.SaveChanges();
                TempData["Success"] = "Đăng ký thành công!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi lưu dữ liệu: " + ex.Message;
                return View(user);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Đăng xuất thành công.";
            return RedirectToAction("Login");
        }

    }
}
