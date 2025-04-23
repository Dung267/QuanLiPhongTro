using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "ChuTro")]
    public class HopDongController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HopDongController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ========================= DANH SÁCH ============================
        public async Task<IActionResult> DanhSach()
        {
            var list = await _context.HopDongs
                .Include(h => h.Phong)
                .Include(h => h.User) // chỉ cần include IdentityUser nếu cần tên đăng nhập
                .ToListAsync();

            return View(list);
        }

        //// ========================= THÊM ============================
        //[HttpGet]
        //public async Task<IActionResult> Them()
        //{
        //    var vm = new HopDongViewModel
        //    {
        //        DanhSachNguoiThue = await _context.NguoiThues
        //            .Select(n => new SelectListItem
        //            {
        //                Value = n.UserId,
        //                Text = n.User.UserName
        //            }).ToListAsync(),
        //        DanhSachPhong = await _context.Phongs
        //            .Select(p => new SelectListItem
        //            {
        //                Value = p.Id.ToString(),
        //                Text = p.TenPhong
        //            }).ToListAsync(),
        //        NgayBatDau = DateTime.Now
        //    };

        //    return View(vm);
        //}
        // Thêm method mới để lấy danh sách người dùng chưa có hợp đồng
        private async Task<List<SelectListItem>> GetAvailableUsers()
        {
            // Lấy tất cả người dùng
            var allUsers = await _userManager.Users.ToListAsync();

            // Lấy danh sách UserId đã có hợp đồng
            var usersWithContracts = await _context.HopDongs
                .Select(h => h.UserId)
                .Distinct()
                .ToListAsync();

            // Lọc ra những người dùng chưa có hợp đồng
            var availableUsers = allUsers
                .Where(u => !usersWithContracts.Contains(u.Id))
                .Select(u => new SelectListItem
                {
                    Value = u.Id,
                    Text = $"{u.UserName} - {u.Email}"
                })
                .ToList();

            return availableUsers;
        }

        // Sửa lại action Them (GET)
        [HttpGet]
        public async Task<IActionResult> Them()
        {
            var vm = new HopDongViewModel
            {
                DanhSachNguoiThue = await GetAvailableUsers(),
                DanhSachPhong = await _context.Phongs
                    .Where(p => p.DaThue== false) // Chỉ hiển thị phòng trống
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.TenPhong} - {p.GiaTien.ToString("N0")} đ"
                    }).ToListAsync(),
                NgayBatDau = DateTime.Now
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(HopDongViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownData(vm);
                return View(vm);
            }

            var hopDong = new HopDong
            {
                UserId = vm.UserId,
                PhongId = vm.PhongId,
                NgayBatDau = vm.NgayBatDau,
                NgayKetThuc = vm.NgayKetThuc,
                TienCoc = vm.TienCoc,
                DaTra = false
            };

            _context.HopDongs.Add(hopDong);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thêm hợp đồng thành công.";
            return RedirectToAction("DanhSach");
        }

        // ========================= SỬA ============================
        [HttpGet]
        public async Task<IActionResult> Sua(int id)
        {
            var hopDong = await _context.HopDongs.FindAsync(id);
            if (hopDong == null) return NotFound();

            var vm = new HopDongViewModel
            {
                Id = hopDong.Id,
                UserId = hopDong.UserId,
                PhongId = hopDong.PhongId,
                NgayBatDau = hopDong.NgayBatDau,
                NgayKetThuc = hopDong.NgayKetThuc,
                TienCoc = hopDong.TienCoc
            };

            await LoadDropdownData(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sua(int id, HopDongViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdownData(vm);
                return View(vm);
            }

            var hopDong = await _context.HopDongs.FindAsync(id);
            if (hopDong == null) return NotFound();

            hopDong.UserId = vm.UserId;
            hopDong.PhongId = vm.PhongId;
            hopDong.NgayBatDau = vm.NgayBatDau;
            hopDong.NgayKetThuc = vm.NgayKetThuc;
            hopDong.TienCoc = vm.TienCoc;

            await _context.SaveChangesAsync();

            TempData["Success"] = "Sửa hợp đồng thành công.";
            return RedirectToAction("DanhSach");
        }

        // ========================= XÓA ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Xoa(int id)
        {
            var hopDong = await _context.HopDongs.FindAsync(id);
            if (hopDong == null) return NotFound();

            var userId = hopDong.UserId;

            // Xóa hợp đồng
            _context.HopDongs.Remove(hopDong);
            await _context.SaveChangesAsync();

            // Kiểm tra người dùng có còn hợp đồng nào khác không
            var hasOtherContracts = await _context.HopDongs.AnyAsync(h => h.UserId == userId);
            if (!hasOtherContracts)
            {
                var nguoiThue = await _context.NguoiThues.FirstOrDefaultAsync(n => n.UserId == userId);
                if (nguoiThue != null)
                {
                    var user = await _context.Users.FindAsync(userId);
                    _context.NguoiThues.Remove(nguoiThue);
                    if (user != null)
                        _context.Users.Remove(user);

                    await _context.SaveChangesAsync();
                }
            }

            TempData["Success"] = "Đã xóa hợp đồng và người thuê (nếu không còn hợp đồng khác).";
            return RedirectToAction("DanhSach");
        }

        // ========================= DROPDOWN DATA ============================
        private async Task LoadDropdownData(HopDongViewModel vm)
        {
            vm.DanhSachNguoiThue = await _context.NguoiThues
                .Select(n => new SelectListItem
                {
                    Value = n.UserId,
                    Text = n.User.UserName
                }).ToListAsync();

            vm.DanhSachPhong = await _context.Phongs
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.TenPhong
                }).ToListAsync();
        }
    }
}
