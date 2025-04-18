using Microsoft.AspNetCore.Identity;

namespace QuanLiPhongTro.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string ? HoTen { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }

    }
}
