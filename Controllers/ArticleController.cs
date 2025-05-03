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

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var role = HttpContext.Session.GetString("Role");

            var articles = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics)
                    .ThenInclude(at => at.Topic);

            if (role == "Admin")
            {
                return View(articles.ToList());
            }
            else
            {
                return View(articles.Where(a => a.AuthorId == userId).ToList());
            }
        }

        public IActionResult Create()
        {
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
            if (ModelState.IsValid)
            {
                var article = new Article
                {
                    Title = model.Article.Title,
                    Summary = model.Article.Summary,
                    Content = model.Article.Content,
                    AuthorId = HttpContext.Session.GetInt32("UserId").Value,
                    SubmittedDate = DateTime.Now,
                    Status = ArticleStatus.Pending,
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
            var article = _context.Articles
                .Include(a => a.ArticleTopics)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null) return NotFound();

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
                .Include(a => a.ArticleTopics)
                .FirstOrDefault(a => a.ArticleId == id);

            if (article == null)
                return NotFound();

            _context.Articles.Remove(article);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Approve(int id)
        {
            var article = _context.Articles.Find(id);
            if (article == null) return NotFound();

            article.Status = ArticleStatus.Approved;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Reject(int id)
        {
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

            var query = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics).ThenInclude(at => at.Topic)
                .Where(a => a.Status == status);

            // Nếu không phải admin thì chỉ cho xem bài của chính mình
            if (role != "Admin" && userId.HasValue)
            {
                query = query.Where(a => a.AuthorId == userId.Value);
            }

            ViewBag.CurrentStatus = status.ToString();
            return View("Index", query.ToList());
        }

    }
}
