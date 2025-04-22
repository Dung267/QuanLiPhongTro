using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("HoaDon")]
    public class HoaDon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ThanhToanId { get; set; }

        [ForeignKey("ThanhToanId")]
        public ThanhToan ThanhToan { get; set; }

        [Required]
        [Display(Name = "Ngày Lập")]
        [DataType(DataType.Date)]
        public DateTime NgayLap { get; set; }

        [Required(ErrorMessage = "Người lập hóa đơn là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên người lập không vượt quá 100 ký tự")]
        [Display(Name = "Người Lập")]
        public string NguoiLap { get; set; }

        [StringLength(255)]
        [Display(Name = "Ghi Chú")]
        public string? GhiChu { get; set; }
    }
}
