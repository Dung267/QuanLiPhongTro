using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Phong")]
    public class Phong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tên Phòng")]
        public string TenPhong { get; set; }

        [Required]
        [Display(Name = "Số Người Tối Đa")]
        public int SoNguoiToiDa { get; set; }

        [Required]
        [Display(Name = "Giá Tiền")]
        [DataType(DataType.Currency)]
        public decimal GiaTien { get; set; }

        [Display(Name = "Đã Cho Thuê")]
        public bool DaChoThue { get; set; } = false;

        [Required]
        public string ToaNhaId { get; set; }

        [ForeignKey("ToaNhaId")]
        public virtual ToaNha ToaNha { get; set; }
        public ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();

    }
}