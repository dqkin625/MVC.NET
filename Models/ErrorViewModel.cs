namespace QuanLyBaiBaoKhoaHoc.Models;

public class ErrorViewModel
{
    public string? RequestId { get; set; } //Mã yêu cầu (Request ID) được tạo tự động bởi ASP.NET Core khi có lỗi xảy ra, dấu ? để cho phép RequestId có thể null (không có giá trị) nếu không có lỗi xảy ra

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); //trả về true nếu RequestId không null hoặc không rỗng, tức là có lỗi xảy ra và cần hiển thị mã yêu cầu cho người dùng
}
