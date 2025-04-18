using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("NguoiThue")]
    public class NguoiThue
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại đến bảng Identity User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string CCCD { get; set; }

        public string SDT { get; set; }


        // Liên kết hợp đồng thuê nếu cần
        public ICollection<HopDong> HopDongs { get; set; }
    }
}
