using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace QuanLiPhongTro.Models
{
    public class HopDongViewModel
    {
            public int? Id { get; set; }

            [Required]
            public string UserId { get; set; }

            [Required]
            public String PhongId { get; set; }

            [Required]
            [DataType(DataType.Date)]
            public DateTime NgayBatDau { get; set; }

            [DataType(DataType.Date)]
            public DateTime NgayKetThuc { get; set; }

            [Required]
            public decimal TienCoc { get; set; }

            public bool DaTra { get; set; }

            public List<SelectListItem> DanhSachNguoiThue { get; set; }
            public List<SelectListItem> DanhSachPhong { get; set; }
        }

}


