using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("ToaNha")]
    public class ToaNha
    {
        [Key]

        [Required(ErrorMessage = "Mã Tòa Nhà là bắt buộc")]
        [Display(Name = "Mã Tòa Nhà")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Tên tòa nhà không được để trống")]
        [Display(Name = "Tên Tòa Nhà")]
        public string TenToa { get; set; }

        public virtual ICollection<Phong> Phongs { get; set; }
    }
}