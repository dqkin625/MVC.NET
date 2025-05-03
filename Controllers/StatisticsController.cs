using Microsoft.AspNetCore.Mvc;
using QuanLyBaiBaoKhoaHoc.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanLyBaiBaoKhoaHoc.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }

            var stats = new StatisticsViewModel
            {
                ArticlesByAuthor = _context.Articles
                    .Include(a => a.Author)
                    .GroupBy(a => a.Author.FullName)
                    .Select(g => new AuthorStats { AuthorName = g.Key, Count = g.Count() })
                    .ToList(),

                ArticlesByTopic = _context.ArticleTopics
                    .Include(at => at.Topic)
                    .GroupBy(at => at.Topic.Name)
                    .Select(g => new TopicStats { TopicName = g.Key, Count = g.Count() })
                    .ToList(),

                ArticlesByStatus = _context.Articles
                    .GroupBy(a => a.Status)
                    .Select(g => new StatusStats { Status = g.Key.ToString(), Count = g.Count() })
                    .ToList()
            };

            return View(stats);
        }
    }

    public class StatisticsViewModel
    {
        public List<AuthorStats> ArticlesByAuthor { get; set; }
        public List<TopicStats> ArticlesByTopic { get; set; }
        public List<StatusStats> ArticlesByStatus { get; set; }
    }

    public class AuthorStats
    {
        public string AuthorName { get; set; }
        public int Count { get; set; }
    }

    public class TopicStats
    {
        public string TopicName { get; set; }
        public int Count { get; set; }
    }

    public class StatusStats
    {
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
