namespace QuanLyBaiBaoKhoaHoc.Models
{
    public class StatisticsViewModel
    {
        public int TotalArticles { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }

        public TopicStats? TopTopic { get; set; }
        public AuthorStats? TopAuthorByApproved { get; set; }
        public List<AuthorStats>? TopAuthorsByApprovedCount { get; set; } // 10 người nhiều bài duyệt nhất
        public List<AuthorFullStats>? TopAuthorsByFullStats { get; set; } // 10 người tổng hợp cả trạng thái

        public List<AuthorStats>? ArticlesByAuthor { get; set; }
        public List<TopicStats>? ArticlesByTopic { get; set; }
        public List<StatusStats>? ArticlesByStatus { get; set; }
    }


    public class AuthorStats
    {
        public string AuthorName { get; set; } = "";
        public int Count { get; set; }
    }

    public class TopicStats
    {
        public string TopicName { get; set; } = "";
        public int Count { get; set; }
    }

    public class StatusStats
    {
        public string Status { get; set; } = "";
        public int Count { get; set; }
    }

    //public class AuthorApprovalStats
    //{
    //    public string AuthorName { get; set; } = "";
    //    public int Total { get; set; }
    //    public int Approved { get; set; }
    //    public double ApprovalRate { get; set; }
    //}

    public class AuthorFullStats
    {
        public string AuthorName { get; set; } = "";
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Pending { get; set; }
    }
    
}
