using Microsoft.AspNetCore.Mvc;
using QuanLyBaiBaoKhoaHoc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace QuanLyBaiBaoKhoaHoc.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _context;

        public ArticleController(AppDbContext context)
        {
            _context = context;
        }
        //Truy cập CSDL thông qua DI (Dependency Injection) để lấy dữ liệu từ DB

        public IActionResult Index(DateTime? fromDate, DateTime? toDate) // Thêm tham số từ ngày và đến ngày
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account"); // Kiểm tra ngay nếu người dùng chưa đăng nhập thì chuyển đến trang đăng nhập

            var role = HttpContext.Session.GetString("Role"); // Lấy từ Session role và id

            var articles = _context.Articles // LinQ Expression lấy các bài viết
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics)
                    .ThenInclude(at => at.Topic)
                .OrderByDescending(a => a.SubmittedDate);

            // Lọc bài viết theo ngày nếu có từ ngày và đến ngày
            if (fromDate.HasValue)
            {
                articles = (IOrderedQueryable<Article>)articles.Where(a => a.SubmittedDate >= fromDate.Value.Date); // Lọc theo từ ngày
            }

            if (toDate.HasValue)
            {
                articles = (IOrderedQueryable<Article>)articles.Where(a => a.SubmittedDate <= toDate.Value.Date); // Lọc theo đến ngày
            }

            // Chuyển đổi về List để tránh lỗi
            var articlesList = articles.ToList();

            // Quản lý quyền truy cập: Admin xem toàn bộ bài viết, Người dùng chỉ xem bài của mình
            if (role == "Admin")
            {
                return View(articlesList); // Admin xem toàn bộ bài viết
            }
            else
            {
                return View(articlesList.Where(a => a.AuthorId == userId).ToList()); // Chỉ hiển thị bài viết của chính mình
            }
        }

        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            // Chưa đăng nhập thì chuyển về Login
            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (role != "Author")
                return RedirectToAction("AccessDenied", "Account");

            var model = new ArticleCreateViewModel
            {
                Topics = _context.Topics
                    .Select(t => new SelectListItem
                    {
                        Value = t.TopicId.ToString(),
                        Text = t.Name
                    }).ToList()
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult Create(ArticleCreateViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var role = HttpContext.Session.GetString("Role");
            if (role != "Author")
                return RedirectToAction("AccessDenied", "Account");

            if (ModelState.IsValid) //kiểm tra các trường có được điền đầy đủ không
            {
                var article = new Article
                {
                    Title = model.Article.Title,
                    Summary = model.Article.Summary,
                    Content = model.Article.Content,
                    //Khi submit form thì sẽ tự động bind vào model Article
                    AuthorId = HttpContext.Session.GetInt32("UserId").Value, //gán id của tác giả là id của người đang đăng nhập
                    SubmittedDate = DateTime.Now,
                    Status = ArticleStatus.Pending, //trạng thái mặc định là đang chờ duyệt
                    ArticleTopics = model.SelectedTopicIds.Select(id => new ArticleTopic
                    {
                        TopicId = id
                    }).ToList()
                };

                _context.Articles.Add(article);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Load lại danh sách chủ đề nếu có lỗi
            model.Topics = _context.Topics.Select(t => new SelectListItem
            {
                Value = t.TopicId.ToString(),
                Text = t.Name
            }).ToList();

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (role != "Author")
                return RedirectToAction("AccessDenied", "Account");

            var article = _context.Articles
                .Include(a => a.ArticleTopics)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null) return NotFound();

            // Nếu không phải admin và không phải tác giả của bài viết chặn truy cập
            if (role != "Admin" && article.AuthorId != userId)
                return RedirectToAction("AccessDenied", "Account");

            var viewModel = new ArticleEditViewModel
            {
                Article = article,
                SelectedTopicIds = article.ArticleTopics.Select(at => at.TopicId).ToList(),
                Topics = _context.Topics.Select(t => new SelectListItem
                {
                    Value = t.TopicId.ToString(),
                    Text = t.Name
                }).ToList()
            };

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult Edit(ArticleEditViewModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            if (role != "Author")
                return RedirectToAction("AccessDenied", "Account");

            if (!ModelState.IsValid)
            {
                model.Topics = _context.Topics.Select(t => new SelectListItem
                {
                    Value = t.TopicId.ToString(),
                    Text = t.Name
                }).ToList();
                return View(model);
            }

            var article = _context.Articles
                .Include(a => a.ArticleTopics)
                .FirstOrDefault(a => a.ArticleId == model.Article.ArticleId);

            if (article == null)
                return NotFound();

            //Kiểm tra phân quyền: chỉ Admin hoặc tác giả mới được sửa
            if (role != "Admin" && article.AuthorId != userId)
                return RedirectToAction("AccessDenied", "Account");

            // Cập nhật lại thông tin cơ bản
            article.Title = model.Article.Title;
            article.Summary = model.Article.Summary;
            article.Content = model.Article.Content;
            article.Status = model.Article.Status;

            // Cập nhật các topic mới
            article.ArticleTopics.Clear();
            foreach (var topicId in model.SelectedTopicIds)
            {
                article.ArticleTopics.Add(new ArticleTopic { TopicId = topicId });
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            // Chưa đăng nhập → chuyển về Login
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var article = _context.Articles
                .Include(a => a.ArticleTopics)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
                return NotFound();

            // ❗ Phân quyền: chỉ Admin hoặc tác giả mới được xóa
            if (role != "Admin" && article.AuthorId != userId)
                return RedirectToAction("AccessDenied", "Account");

            _context.Articles.Remove(article);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Approve(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                return RedirectToAction("AccessDenied", "Account"); // Hoặc return Forbid();

            var article = _context.Articles.Find(id);
            if (article == null) return NotFound();

            article.Status = ArticleStatus.Approved;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Reject(int id)
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin")
                return RedirectToAction("AccessDenied", "Account");

            var article = _context.Articles.Find(id);
            if (article == null) return NotFound();

            article.Status = ArticleStatus.Rejected;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var article = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics)
                    .ThenInclude(at => at.Topic)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
                return NotFound();

            // Nếu không phải Admin và không phải tác giả → chặn truy cập
            if (role != "Admin" && article.AuthorId != userId)
                return RedirectToAction("AccessDenied", "Account");

            return PartialView("ArticleDetailsPartial", article);
        }

        public IActionResult IndexByStatus(ArticleStatus status)
        {
            var role = HttpContext.Session.GetString("Role");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var query = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics).ThenInclude(at => at.Topic)
                .Where(a => a.Status == status);

            // Nếu là Author thì chỉ xem bài của chính mình
            if (role != "Admin")
            {
                query = query.Where(a => a.AuthorId == userId);
            }

            ViewBag.CurrentStatus = status.ToString();

            // Sắp xếp theo ngày gửi mới nhất
            return View("Index", query
                .OrderByDescending(a => a.SubmittedDate)
                .ToList());
        }
    }
}
