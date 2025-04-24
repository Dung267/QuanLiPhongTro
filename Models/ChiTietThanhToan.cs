using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("ChiTietThanhToan")]
    public class ChiTietThanhToan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ThanhToanId { get; set; }

        [ForeignKey("ThanhToanId")]
        public ThanhToan ThanhToan { get; set; }

        [Required(ErrorMessage = "Loại khoản thanh toán là bắt buộc")]
        [StringLength(20)]
        public string Loai { get; set; } // "TienPhong" hoặc "DichVu"

        [Required(ErrorMessage = "Mô tả là bắt buộc")]
        [StringLength(255)]
        public string MoTa { get; set; }

        [Required]
        [Range(0, 999999999, ErrorMessage = "Số tiền không hợp lệ")]
        [DataType(DataType.Currency)]
        public decimal SoTien { get; set; }
    }
}