using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("SuCo")]
    public class SuCo
    {
        [Key]
        public int Id { get; set; }
        public string MoTa { get; set; }
        public DateTime NgayBaoCao { get; set; }
        public bool DaGiaiQuyet { get; set; }

        [Required]
        [Display(Name = "Phòng Gặp Sự Cố")]
        public String PhongId { get; set; }

        [ForeignKey("PhongId")]
        public Phong Phong { get; set; }
    }
}
