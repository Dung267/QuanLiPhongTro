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
                    .Where(p => p.DaChoThue== false) // Chỉ hiển thị phòng trống
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
                vm.DanhSachNguoiThue = await GetAvailableUsers();
                vm.DanhSachPhong = await _context.Phongs
                    .Where(p => p.DaChoThue == false)
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = $"{p.TenPhong} - {p.GiaTien.ToString("N0")} đ"
                    }).ToListAsync();
                return View(vm);
            }

            // Kiểm tra phòng có sẵn không
            var phong = await _context.Phongs.FindAsync(vm.PhongId);
            if (phong == null || phong.DaChoThue)
            {
                ModelState.AddModelError("PhongId", "Phòng này đã được cho thuê");
                await LoadDropdownData(vm);
                return View(vm);
            }

            // Tạo hợp đồng
            var hopDong = new HopDong
            {
                UserId = vm.UserId,
                PhongId = vm.PhongId,
                NgayBatDau = vm.NgayBatDau,
                NgayKetThuc = vm.NgayKetThuc,
                TienCoc = vm.TienCoc,
                DaTra = false
            };

            // Cập nhật trạng thái phòng
            phong.DaChoThue = true;
            _context.Phongs.Update(phong);

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

            // Kiểm tra nếu thay đổi phòng
            if (hopDong.PhongId != vm.PhongId)
            {
                // Cập nhật trạng thái phòng cũ
                var phongCu = await _context.Phongs.FindAsync(hopDong.PhongId);
                if (phongCu != null)
                {
                    phongCu.DaChoThue = false;
                    _context.Phongs.Update(phongCu);
                }

                // Cập nhật trạng thái phòng mới
                var phongMoi = await _context.Phongs.FindAsync(vm.PhongId);
                if (phongMoi == null || phongMoi.DaChoThue)
                {
                    ModelState.AddModelError("PhongId", "Phòng này đã được cho thuê");
                    await LoadDropdownData(vm);
                    return View(vm);
                }
                phongMoi.DaChoThue = true;
                _context.Phongs.Update(phongMoi);
            }

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
            var phongId = hopDong.PhongId;

            // Xóa hợp đồng
            _context.HopDongs.Remove(hopDong);

            // Cập nhật trạng thái phòng
            var phong = await _context.Phongs.FindAsync(phongId);
            if (phong != null)
            {
                phong.DaChoThue = false;
                _context.Phongs.Update(phong);
            }

            await _context.SaveChangesAsync();

            // Kiểm tra người dùng có còn hợp đồng nào khác không
            var hasOtherContracts = await _context.HopDongs.AnyAsync(h => h.UserId == userId);
            if (!hasOtherContracts)
            {
                var nguoiThue = await _context.NguoiThues.FirstOrDefaultAsync(n => n.UserId == userId);
                if (nguoiThue != null)
                {
                    _context.NguoiThues.Remove(nguoiThue);
                    await _context.SaveChangesAsync();
                }
            }

            TempData["Success"] = "Đã xóa hợp đồng và cập nhật trạng thái phòng.";
            return RedirectToAction("DanhSach");
        }

        // ========================= DROPDOWN DATA ============================
        private async Task LoadDropdownData(HopDongViewModel vm)
        {
            vm.DanhSachNguoiThue = await GetAvailableUsers();
            vm.DanhSachPhong = await _context.Phongs
                .Where(p => p.DaChoThue == false || p.Id == vm.PhongId) // Hiển thị cả phòng đang chọn (khi sửa)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = $"{p.TenPhong} - {p.GiaTien.ToString("N0")} đ",
                    Selected = p.Id == vm.PhongId
                }).ToListAsync();
        }
    }
}
