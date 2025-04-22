using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("DichVu")]
    public class DichVu
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên dịch vụ là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên không vượt quá 100 ký tự")]
        [Display(Name = "Tên Dịch Vụ")]
        public string TenDichVu { get; set; }

        [Required(ErrorMessage = "Đơn giá là bắt buộc")]
        [Range(0, 999999999, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        [Display(Name = "Đơn Giá")]
        [DataType(DataType.Currency)]
        public decimal DonGia { get; set; }

        public ICollection<SuDungDichVu> SuDungDichVus { get; set; } = new List<SuDungDichVu>();
    }
}
