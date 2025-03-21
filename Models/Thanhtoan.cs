using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Thanhtoan")]
    public class Thanhtoan
    {
        [Key]
        public int ID { get; set; }
        public int SoTien { get; set; }
        public DateTime HanThanhToan { get; set; }
        public String trangThai { get; set; }// thanh toan chua thanh toan


    }
}
