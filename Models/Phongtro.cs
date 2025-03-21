using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Phongtro")]
    public class Phongtro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string TheLoai { get; set; }

        [Required]
        public string DienTich { get; set; }

        [Required]
        public string MoTa { get; set; }

        [Required]
        public int Gia { get; set; }

        [Required]
        public string TrangThai { get; set; }
    }
}
