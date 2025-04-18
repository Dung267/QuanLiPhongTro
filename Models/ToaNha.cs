using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuanLiPhongTro.Models
{
    [Table("ToaNha")]
    public class ToaNha
    {
        [Key]
        public int Id { get; set; }
        public string TenToa { get; set; }

        public ICollection<Phong> Phongs { get; set; }
    }
}
