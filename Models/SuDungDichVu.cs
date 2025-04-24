using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("SuDungDichVu")]
    public class SuDungDichVu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Chỉ Số Cũ")]
        [Range(0, int.MaxValue, ErrorMessage = "Chỉ số cũ không hợp lệ")]
        public int ChiSoCu { get; set; }

        [Required]
        [Display(Name = "Chỉ Số Mới")]
        [Range(0, int.MaxValue, ErrorMessage = "Chỉ số mới không hợp lệ")]
        public int ChiSoMoi { get; set; }

        [Required]
        [Display(Name = "Tháng/Năm")]
        [DataType(DataType.Date)]
        public DateTime ThangNam { get; set; }

        [Required]
        public int DichVuId { get; set; }

        [ForeignKey("DichVuId")]
        public DichVu DichVu { get; set; }

        [Required]
        public String PhongId { get; set; }

        [ForeignKey("PhongId")]
        public Phong Phong { get; set; }
    }
}