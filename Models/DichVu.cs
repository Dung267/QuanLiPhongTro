using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("DichVu")]
    public class DichVu
    {
        [Key]
        public int Id { get; set; }
        public string TenDichVu { get; set; }

        public decimal DonGia { get; set; }

        public ICollection<SuDungDichVu> SuDungDichVus { get; set; }
    }
}
