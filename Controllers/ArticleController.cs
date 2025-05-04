using Microsoft.AspNetCore.Mvc;
using QuanLyBaiBaoKhoaHoc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index() //hiển thị danh sách bài viết
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account"); //kiểm tra ngay nếu người dùng chưa đăng nhập thì chuyển đến trang đăng nhập
            var role = HttpContext.Session.GetString("Role"); //Lấy từ Session role và id

            var articles = _context.Articles //LinQ Expression 1 tập các bài viết
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics)
                    .ThenInclude(at => at.Topic);
            // với mỗi bài viết a nạp thêm thuộc tính a.Author(tác giả bài viết đó) (a là biến ẩn danh đại diện cho đối tượng Article trong quá trình LINQ xử lý)

            if (role == "Admin")
            {
                return View(articles.ToList()); //admin xem toàn bộ bài viết
            }
            else
            {
                return View(articles.Where(a => a.AuthorId == userId).ToList()); //chỉ hiển thị bài viết của chính mình
            }
        }

        public IActionResult Create()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            // Chưa đăng nhập → chuyển về Login
            if (userId == null)
                return RedirectToAction("Login", "Account");

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

            // Chưa đăng nhập → chuyển về Login
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var article = _context.Articles
                .Include(a => a.ArticleTopics)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null) return NotFound();

            // Nếu không phải admin và không phải tác giả của bài viết → chặn truy cập
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
            if (userId == null)
                return RedirectToAction("Login", "Account");

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
                .Include(a => a.ArticleTopics) // nạp các chủ đề liên quan đến bài viết
                .FirstOrDefault(a => a.ArticleId == model.Article.ArticleId); //tìm đúng bài viết cần sửa

            if (article == null) return NotFound();

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
            var article = _context.Articles
                .Include(a => a.ArticleTopics) //phải include để xóa luôn các liên kết trong bảng ArticleTopic (tránh lỗi quan hệ ngoại)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
                return NotFound();

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
            var article = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics)
                    .ThenInclude(at => at.Topic)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
                return NotFound();

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

            // ✅ Nếu là Author thì chỉ xem bài của chính mình
            if (role != "Admin")
            {
                query = query.Where(a => a.AuthorId == userId);
            }

            ViewBag.CurrentStatus = status.ToString();
            return View("Index", query.ToList());
        }
    }
}
