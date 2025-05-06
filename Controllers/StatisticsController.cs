using Microsoft.AspNetCore.Mvc;
using QuanLyBaiBaoKhoaHoc.Models;
using Microsoft.EntityFrameworkCore;

namespace QuanLyBaiBaoKhoaHoc.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly AppDbContext _context;

        public StatisticsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
                return RedirectToAction("Login", "Account");

            if (HttpContext.Session.GetString("Role") != "Admin")
                return RedirectToAction("AccessDenied", "Account");

            // Lọc theo ngày gửi nếu có
            var articlesQuery = _context.Articles
                .Include(a => a.Author)
                .Include(a => a.ArticleTopics)
                .ThenInclude(at => at.Topic)
                .AsQueryable();

            if (fromDate.HasValue)
                articlesQuery = articlesQuery.Where(a => a.SubmittedDate >= fromDate.Value);
            if (toDate.HasValue)
                articlesQuery = articlesQuery.Where(a => a.SubmittedDate <= toDate.Value);

            var articles = articlesQuery.ToList();

            var stats = new StatisticsViewModel
            {
                TotalArticles = articles.Count,
                PendingCount = articles.Count(a => a.Status == ArticleStatus.Pending),
                ApprovedCount = articles.Count(a => a.Status == ArticleStatus.Approved),
                RejectedCount = articles.Count(a => a.Status == ArticleStatus.Rejected),

                TopTopic = articles
                    .Where(a => a.Status == ArticleStatus.Approved)
                    .SelectMany(a => a.ArticleTopics)
                    .GroupBy(at => at.Topic.Name)
                    .Select(g => new TopicStats { TopicName = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .FirstOrDefault(),

                TopAuthorByApproved = articles
                    .Where(a => a.Status == ArticleStatus.Approved)
                    .GroupBy(a => a.Author.FullName)
                    .Select(g => new AuthorStats { AuthorName = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .FirstOrDefault(),

                TopAuthorsByFullStats = articles
                    .GroupBy(a => a.Author.FullName)
                    .Select(g => new AuthorFullStats
                    {
                        AuthorName = g.Key,
                        Total = g.Count(),
                        Approved = g.Count(a => a.Status == ArticleStatus.Approved),
                        Rejected = g.Count(a => a.Status == ArticleStatus.Rejected),
                        Pending = g.Count(a => a.Status == ArticleStatus.Pending)
                    })
                    .OrderByDescending(x => x.Total)
                    .Take(10)
                    .ToList(),

                ArticlesByAuthor = articles
                    .Where(a => a.Status == ArticleStatus.Approved)
                    .GroupBy(a => a.Author.FullName)
                    .Select(g => new AuthorStats { AuthorName = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .ToList(),

                ArticlesByTopic = articles
                    .Where(a => a.Status == ArticleStatus.Approved)
                    .SelectMany(a => a.ArticleTopics)
                    .GroupBy(at => at.Topic.Name)
                    .Select(g => new TopicStats { TopicName = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .ToList(),

                ArticlesByStatus = articles
                    .GroupBy(a => a.Status)
                    .Select(g => new StatusStats { Status = g.Key.ToString(), Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .ToList(),

                TopAuthorsByApprovedCount = articles
                    .Where(a => a.Status == ArticleStatus.Approved)
                    .GroupBy(a => a.Author.FullName)
                    .Select(g => new AuthorStats { AuthorName = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .Take(10)
                    .ToList(),
            };

            return View(stats);
        }


    }
}
