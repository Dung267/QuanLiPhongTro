using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public int Id { get; set; }

        public int ThanhToanId { get; set; }
        public ThanhToan ThanhToan { get; set; }

        public DateTime NgayLap { get; set; } // Ngày lập hóa đơn

        public string NguoiLap { get; set; } // Nguoi lập hóa đơn

        public string GhiChu { get; set; }
    }
}
