using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<QuanLiPhongTro.Models.NguoiThue> NguoiThues { get; set; }
        public DbSet<QuanLiPhongTro.Models.Phong> Phongs { get; set; }
        public DbSet<QuanLiPhongTro.Models.ToaNha> ToaNhas { get; set; }
        public DbSet<QuanLiPhongTro.Models.DichVu> DichVus { get; set; }
        public DbSet<QuanLiPhongTro.Models.SuDungDichVu> SuDungDichVus { get; set; }
        public DbSet<QuanLiPhongTro.Models.HopDong> HopDongs { get; set; }
        public DbSet<QuanLiPhongTro.Models.HoaDon> HoaDons { get; set; }
        public DbSet<QuanLiPhongTro.Models.TraHopDong> TraHopDongs { get; set; }
        public DbSet<QuanLiPhongTro.Models.SuCo> SuCos { get; set; }
        public DbSet<QuanLiPhongTro.Models.ThanhToan> ThanhToans { get; set; }
        public DbSet<QuanLiPhongTro.Models.ChiTietThanhToan> ChiTietThanhToans { get; set; }

        
    }
}
