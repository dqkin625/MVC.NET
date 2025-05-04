using QuanLyBaiBaoKhoaHoc.Models;

public class ArticleTopic //bảng trung gian
{
    public int ArticleId { get; set; } //Khóa ngoại tham chiếu đến Article.ArticleId
    public Article Article { get; set; } //navigation property có thể truy cập chi tiết thông tin của Article từ 1 dòng trong ArticleTopic

    public int TopicId { get; set; } //Khóa ngoại tham chiếu đến Topic.TopicId
    public Topic Topic { get; set; } //navigation property
}
