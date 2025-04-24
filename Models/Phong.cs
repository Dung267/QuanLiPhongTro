using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("Phong")]
    public class Phong
    {
        [Key]
        [Required(ErrorMessage = "Mã phòng là bắt buộc")]
        [Display(Name = "Mã Phòng")]
        [RegularExpression(@"^[A-Za-z]{1,3}\d{1,4}$", ErrorMessage = "Mã phòng phải có định dạng như P01, A101, PN02...")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Tên phòng không được để trống")]
        [Display(Name = "Tên Phòng")]
        public string TenPhong { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số người tối đa")]
        [Range(1, 20, ErrorMessage = "Số người tối đa phải từ 1 đến 20")]
        [Display(Name = "Số Người Tối Đa")]
        public int SoNguoiToiDa { get; set; }

        [Required(ErrorMessage = "Giá tiền không được để trống")]
        [Range(100000, 10000000, ErrorMessage = "Giá tiền phải từ 100.000 đến 10.000.000")]
        [Display(Name = "Giá Tiền")]
        [DataType(DataType.Currency)]
        public decimal GiaTien { get; set; }

        [Display(Name = "Đã Cho Thuê")]
        public bool DaChoThue { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn tòa nhà")]
        [Display(Name = "Mã Tòa Nhà")]
        public string ToaNhaId { get; set; }

        [ForeignKey("ToaNhaId")]
        public virtual ToaNha? ToaNha { get; set; }
        public bool DaThue { get; set; }
        public ICollection<HopDong> HopDongs { get; set; } = new List<HopDong>();
    }
}
