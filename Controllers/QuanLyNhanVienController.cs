/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using System.Threading.Tasks;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "ChuTro")]
    public class QuanLyNhanVienController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public QuanLyNhanVienController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Danh sách nhân viên
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var nhanViens = new List<IdentityUser>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "QuanLi"))
                {
                    nhanViens.Add(user);
                }
            }

            return View(nhanViens);
        }

        // Khóa tài khoản
        public async Task<IActionResult> KhoaTaiKhoan(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = DateTimeOffset.MaxValue;
            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Đã khóa tài khoản.";
            return RedirectToAction("Index");
        }

        // Mở khóa tài khoản
        public async Task<IActionResult> MoKhoaTaiKhoan(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.LockoutEnd = null;
            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Đã mở khóa tài khoản.";
            return RedirectToAction("Index");
        }

        // Reset mật khẩu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetMatKhau(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Đặt lại mật khẩu thành công.";
                return RedirectToAction("Index");
            }

            TempData["Error"] = "Lỗi khi đặt lại mật khẩu.";
            return RedirectToAction("Index");
        }
    }
}
*/

/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "ChuTro")]
    public class NhanVienController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NhanVienController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nhanViens = await _context.Users
                .Where(u => u.Roles.Contains("QuanLi"))
                .Select(u => new NhanVienViewModel
                {
                    UserId = u.Id,
                    Email = u.Email,
                    IsLocked = u.LockoutEnabled,
                    TienDoXuLy = "Chưa xử lý" // Bạn có thể thêm logic cho tiến độ xử lý ở đây
                })
                .ToListAsync();

            return View(nhanViens);
        }

        [HttpPost]
        public async Task<IActionResult> KhoaTaiKhoan(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LockoutEnabled = true;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tài khoản đã được khóa.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ResetMatKhau(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                // Reset mật khẩu logic
                user.PasswordHash = null; // Reset mật khẩu
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Mật khẩu đã được reset.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy tài khoản.";
            }

            return RedirectToAction("Index");
        }
    }
}
*/