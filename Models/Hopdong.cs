using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("HopDong")]
    public class HopDong
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ngày Bắt Đầu")]
        [DataType(DataType.Date)]
        public DateTime NgayBatDau { get; set; }

        [Required]
        [Display(Name = "Ngày Kết Thúc")]
        [DataType(DataType.Date)]
        public DateTime NgayKetThuc { get; set; }

        [Required]
        [Display(Name = "Tiền Cọc")]
        [Range(0, 999999999, ErrorMessage = "Tiền cọc không hợp lệ")]
        [DataType(DataType.Currency)]
        public decimal TienCoc { get; set; }

        [Display(Name = "Đã Trả Phòng")]
        public bool DaTra { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        [Required]
        public String PhongId { get; set; }

        [ForeignKey("PhongId")]
        public Phong Phong { get; set; }

        public ICollection<TraHopDong> TraHopDongs { get; set; } = new List<TraHopDong>();
        public ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
    }
}
