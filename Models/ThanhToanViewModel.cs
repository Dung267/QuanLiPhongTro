using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Models
{
    public class ThanhToanViewModel    {
        public List<ThanhToan> DanhSachThanhToan { get; set; }

        public TrangThaiThanhToan? TrangThai { get; set; }

        public DateTime? TuNgay { get; set; }

        public DateTime? DenNgay { get; set; }

        public List<SelectListItem> TrangThaiList => new List<SelectListItem>
        {
            new SelectListItem { Text = "Tất cả", Value = "" },
            new SelectListItem { Text = "Đã Thanh Toán", Value = "DaThanhToan" },
            new SelectListItem { Text = "Chưa Thanh Toán", Value = "ChuaThanhToan" },
            new SelectListItem { Text = "Đang Chờ Xử Lý", Value = "DangChoXuLy" },
            new SelectListItem { Text = "Đã Hủy", Value = "DaHuy" }
        };
    }
}
