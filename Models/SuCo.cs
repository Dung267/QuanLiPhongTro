using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("SuCo")]
    public class SuCo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mô tả sự cố")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        [Display(Name = "Mô tả sự cố")]
        public string MoTa { get; set; }

        [Display(Name = "Ngày Báo Cáo")]
        [DataType(DataType.DateTime)]
        public DateTime NgayBaoCao { get; set; }

        [Display(Name = "Đã Giải Quyết")]
        public bool DaGiaiQuyet { get; set; }

        [Required]
        [Display(Name = "Phòng Gặp Sự Cố")]
        public String PhongId { get; set; }

        [ForeignKey("PhongId")]
        public Phong Phong { get; set; }
    }
}