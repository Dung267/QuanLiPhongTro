using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("NguoiThue")]
    public class NguoiThue
    {
        [Key]
        [ForeignKey("User")]
        [Display(Name = "Mã Người Dùng")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        [Required(ErrorMessage = "CCCD không được để trống")]
        [StringLength(12, MinimumLength = 9, ErrorMessage = "CCCD phải từ 9 đến 12 số")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CCCD chỉ chứa số")]
        public string CCCD { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(15)]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string SDT { get; set; }

        public ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();
    }
}
