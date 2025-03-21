using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Dichvu")]
    public class Dichvu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TenDV { get; set; }

        [Required]
        public int Gia { get; set; }

        [Required]
        public string MoTa { get; set; }

        [Required]
        public string TrangThai { get; set; }

    }
}
