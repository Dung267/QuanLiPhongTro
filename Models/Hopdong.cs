using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Hopdong")]
    public class Hopdong
    {
        public int ID { get; set; }
        public DateTime NgayBatDau { get; set; }= DateTime.Now;
        public DateTime NgayKetThuc { get; set; }
        public String QuyDinh { get; set; }
        

    }
}
