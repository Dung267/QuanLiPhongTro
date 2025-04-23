using Microsoft.AspNetCore.Identity;

namespace QuanLiPhongTro.Models
{
    public class ThanhToan
    {
        public int Id { get; set; }

        public string UserId { get; set; } // FK đến ApplicationUser
        public IdentityUser User { get; set; }

        public int? HopDongId { get; set; } // Gắn với hợp đồng nào (nếu có)
        public HopDong HopDong { get; set; }
        public TrangThaiThanhToan trangThaiThanhToan { get; set; } // Trạng thái thanh toán (Đã thanh toán, Chưa thanh toán, Đang chờ xử lý, Đã hủy, ...)
        public DateTime NgayThanhToan { get; set; }

        public decimal TongTien { get; set; }

        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; }
    }
    public enum TrangThaiThanhToan
    {
        DaThanhToan,
        ChuaThanhToan,
        DangChoXuLy,
        DaHuy
    }
}
