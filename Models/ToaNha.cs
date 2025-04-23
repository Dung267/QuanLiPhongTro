using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("ToaNha")]
    public class ToaNha
    {
        [Key]

        [Required(ErrorMessage = "Mã Tòa Nhà là bắt buộc")]
        [RegularExpression(@"^[A-Za-z]{1,2}\d{1,4}$", ErrorMessage = "Mã phải là T01, TN02...")]
        [Display(Name = "Mã Tòa Nhà")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Tên tòa nhà không được để trống")]
        [Display(Name = "Tên Tòa Nhà")]
        public string TenToa { get; set; }

        public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();

    }
}
