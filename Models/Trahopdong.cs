using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("TraHopDong")]
    public class TraHopDong
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Ngày Trả Phòng")]
        [DataType(DataType.Date)]
        public DateTime NgayTra { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập lý do trả phòng")]
        [StringLength(255, ErrorMessage = "Lý do không vượt quá 255 ký tự")]
        [Display(Name = "Lý Do Trả")]
        public string LyDo { get; set; }

        [Required]
        public int HopDongId { get; set; }

        [ForeignKey("HopDongId")]
        public HopDong HopDong { get; set; }
    }
}