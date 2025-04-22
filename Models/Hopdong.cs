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
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal TienCoc { get; set; }
        public bool DaTra { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public int PhongId { get; set; }
        public Phong Phong { get; set; }

        public ICollection<TraHopDong> TraHopDongs { get; set; }
        public ICollection<HoaDon> HoaDons { get; set; }

    }
}
