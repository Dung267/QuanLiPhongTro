using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace QuanLiPhongTro.Models
{
    public class HopDongViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public String PhongId { get; set; } // <- Chuyển từ int sang string

        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal TienCoc { get; set; }

        public string CCCD { get; set; }
        public string SDT { get; set; }

        public List<SelectListItem>? DanhSachNguoiThue { get; set; }
        public List<SelectListItem>? DanhSachPhong { get; set; }
    }


}


