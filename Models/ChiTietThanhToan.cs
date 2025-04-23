using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("ChiTietThanhtoan")]
    public class ChiTietThanhToan
    {
        [Key]
        public int Id { get; set; }

        public int ThanhToanId { get; set; } // FK đến ThanhToan
        public ThanhToan ThanhToan { get; set; }

        public string Loai { get; set; } // "TienPhong" hoặc "DichVu"

        public string MoTa { get; set; } // Ví dụ: "Tiền phòng tháng 4", "Điện + nước + mạng"

        public decimal SoTien { get; set; }
    }
}
