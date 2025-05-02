using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    public class ThanhToan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public int? HopDongId { get; set; }

        [ForeignKey("HopDongId")]
        public HopDong HopDong { get; set; }

        [Required]
        [Display(Name = "Trạng Thái Thanh Toán")]
        public TrangThaiThanhToan trangThaiThanhToan { get; set; }

        [Required]
        [Display(Name = "Ngày Thanh Toán")]
        [DataType(DataType.Date)]
        public DateTime NgayThanhToan { get; set; }

        [Required]
        [Range(0, 999999999, ErrorMessage = "Số tiền không hợp lệ")]
        [Display(Name = "Tổng Tiền")]
        [DataType(DataType.Currency)]
        public decimal TongTien { get; set; }

        [Display(Name = "Tháng/Năm")]
        [DataType(DataType.Date)]
        public DateTime ThangNam { get; set; }
        public ICollection<ChiTietThanhToan> ChiTietThanhToans { get; set; } = new List<ChiTietThanhToan>();
    }

    public enum TrangThaiThanhToan
    {
        [Display(Name = "Đã Thanh Toán")]
        DaThanhToan,

        [Display(Name = "Chưa Thanh Toán")]
        ChuaThanhToan,

        [Display(Name = "Đang Chờ Xử Lý")]
        DangChoXuLy,

        [Display(Name = "Đã Hủy")]
        DaHuy
    }
}