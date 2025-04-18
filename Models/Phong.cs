using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Phong")]
    public class Phong
    {
        [Key]
        public int Id { get; set; }
        public string TenPhong { get; set; }
        public int SoNguoiToiDa { get; set; }
        public decimal GiaTien { get; set; }
        public bool DaThue { get; set; }

        public int ToaNhaId { get; set; }
        public ToaNha ToaNha { get; set; }

        public ICollection<HopDong> HopDongs { get; set; }
        public ICollection<SuCo> SuCos { get; set; }
    }
}
