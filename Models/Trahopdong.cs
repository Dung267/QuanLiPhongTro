using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("TraHopDong")]
    public class TraHopDong
    {
        [Key]
        public int Id { get; set; }

        public DateTime NgayTra { get; set; }

        public string LyDo { get; set; }

        public int HopDongId { get; set; }

        public HopDong HopDong { get; set; }

    }
}
