using Microsoft.AspNetCore.Mvc;
using QuanLyBaiBaoKhoaHoc.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace QuanLyBaiBaoKhoaHoc.Controllers
{
    public class AccountController : Controller
    {
        //
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        // Truy cập CSDL thông qua DI (Dependency Injection) để lấy dữ liệu từ DB
        // Hiển thị trang đăng nhập
        public IActionResult Login() //Không cần truyền model vì form đăng nhập chỉ có 2 trường là tài khoản và mật khẩu
        {
            return View();
        }

        // Xử lý đăng nhập
        [HttpPost] //phương thức này sẽ được gọi khi người dùng nhấn nút đăng nhập trên form
        public IActionResult Login(string username, string password) //nhận tên đăng nhập và mật khẩu điền vào form
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == password); 
            //Tìm người dùng trong DbSet Users có tên đăng nhập và mật khẩu khớp với dữ liệu nhập vào form
            //u là mỗi dòng trong bảng User, dùng LINQ để thay thế cho for each (var u in Users)...

            if (user != null)
            {
                // Lưu thông tin vào session để ở các controller khác kiểm tra Session[Role] để phân quyền
                //Session là vùng nhớ tạm thời trên server, lưu trữ thông tin người dùng trong suốt phiên làm việc và lấy ra ở các Controller khác
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
        [HttpPost] //phương thức này sẽ được gọi khi người dùng nhấn nút đăng ký trên form
        public IActionResult Register(User user) //Dữ liệu từ form sẽ được tự động bind vào model User
        {
            if (!ModelState.IsValid) //kiểm tra hợp lệ
            {
                TempData["Error"] = "Dữ liệu nhập chưa hợp lệ.";
                return View(user);
            }

            if (_context.Users.Any(u => u.Username == user.Username || u.Email == user.Email)) //kiểm tra trùng email hoặc username
            {
                ModelState.AddModelError(string.Empty, "Tên tài khoản hoặc Email đã tồn tại.");
                return View(user);
            }

            try
            {
                user.Role = UserRole.Author; //gán quyền tác giả (người viết bài báo)
                _context.Users.Add(user); //thêm người dùng vào DbSet Users
                _context.SaveChanges(); //lưu thay đổi vào CSDL
                TempData["Success"] = "Đăng ký thành công!";
                return RedirectToAction("Login"); //chuyển hướng về trang đăng nhập
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi lưu dữ liệu: " + ex.Message;
                return View(user);
            }
        }

        [HttpGet] //Hiển thị nội dung
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); //xóa toàn bộ thông tin trong session (đăng xuất)
            TempData["Success"] = "Đăng xuất thành công.";
            return RedirectToAction("Login"); //chuyển hướng về trang đăng nhập
        }

    }
}
