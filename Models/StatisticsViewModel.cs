namespace QuanLyBaiBaoKhoaHoc.Models
{
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
