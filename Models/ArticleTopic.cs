using QuanLyBaiBaoKhoaHoc.Models;

public class ArticleTopic
{
    public int ArticleId { get; set; }
    public Article Article { get; set; }

    public int TopicId { get; set; }
    public Topic Topic { get; set; }
}
