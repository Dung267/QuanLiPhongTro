using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("SuCo")]
    public class SuCo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TenSC { get; set; }

        [Required]
        public string MoTa { get; set; }

        [Required]
        public string TrangThai { get; set; }
    }
}
