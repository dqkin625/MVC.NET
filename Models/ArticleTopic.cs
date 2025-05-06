using QuanLyBaiBaoKhoaHoc.Models;

public class ArticleTopic // bảng trung gian
{
    public int ArticleId { get; set; }     // khóa ngoại đến bảng Article
    public Article Article { get; set; }   // navigation property đến Article

    public int TopicId { get; set; }       // khóa ngoại đến bảng Topic
    public Topic Topic { get; set; }       // navigation property đến Topic
}

