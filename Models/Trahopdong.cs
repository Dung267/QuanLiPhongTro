using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Trahopdong")]
    public class Trahopdong
    {
        [Key]
        public int ID { get; set; }
        
        public int SoTien { get; set; }
        public String trangThai { get; set; }
        public Hopdong Hopdong { get; set; }


    }
}
