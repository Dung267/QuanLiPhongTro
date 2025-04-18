using Microsoft.AspNetCore.Identity;
using QuanLiPhongTro.ChucNangPhanQuyen;
using System;
using System.Threading.Tasks;

namespace QuanLiPhongTro.Data
{
    public class DataSeed
    {
        public static async Task KhoiTaoDuLieuMacDinh(IServiceProvider dichVu)
        {
            var quanLyNguoiDung = dichVu.GetService<UserManager<IdentityUser>>();
            var quanLyVaiTro = dichVu.GetService<RoleManager<IdentityRole>>();

            // Tạo các vai trò nếu chưa tồn tại
            foreach (var role in Enum.GetValues(typeof(PhanQuyen)))
            {
                var roleName = role.ToString();
                if (!await quanLyVaiTro.RoleExistsAsync(roleName))
                {
                    await quanLyVaiTro.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Tạo tài khoản chủ trọ
            var chuTroEmail = "chutro@gmail.com";
            var chuTroUser = await quanLyNguoiDung.FindByEmailAsync(chuTroEmail);
            if (chuTroUser == null)
            {
                var taiKhoanChuTro = new IdentityUser
                {
                    UserName = chuTroEmail,
                    Email = chuTroEmail,
                    EmailConfirmed = true
                };

                await quanLyNguoiDung.CreateAsync(taiKhoanChuTro, "ChuTro@123");
                await quanLyNguoiDung.AddToRoleAsync(taiKhoanChuTro, PhanQuyen.ChuTro.ToString());
            }

            //  Tạo tài khoản quản lý 
            var quanLiEmail = "quanli@gmail.com";
            var quanLiUser = await quanLyNguoiDung.FindByEmailAsync(quanLiEmail);
            if (quanLiUser == null)
            {
                var taiKhoanQuanLi = new IdentityUser
                {
                    UserName = quanLiEmail,
                    Email = quanLiEmail,
                    EmailConfirmed = true
                };

                await quanLyNguoiDung.CreateAsync(taiKhoanQuanLi, "QuanLi@123");
                await quanLyNguoiDung.AddToRoleAsync(taiKhoanQuanLi, PhanQuyen.QuanLi.ToString());
            }

           
        }
    }
}
