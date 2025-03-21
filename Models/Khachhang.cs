using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Khachhang")]
    public class Khachhang
    {
        [Required]
        [Key]
        public int ID { get; set; }


    }
}
