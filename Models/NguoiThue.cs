using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("NguoiThue")]
    public class NguoiThue
    {
        [Key] // Dùng luôn UserId làm khóa chính
        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; } // Hoặc IdentityUser nếu bạn không dùng ApplicationUser
        public string CCCD { get; set; }

        public string SDT { get; set; }

        public ICollection<HopDong> HopDongs { get; set; }
    }
}
