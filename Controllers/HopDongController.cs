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
                .Include(h => h.User)
                .ToListAsync();

            return View(list);
        }

        private async Task<List<SelectListItem>> GetAvailableUsers()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var usersWithContracts = await _context.HopDongs
                .Select(h => h.UserId)
                .Distinct()
                .ToListAsync();

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

        [HttpGet]
        public async Task<IActionResult> Them()
        {
            var vm = new HopDongViewModel
            {
                DanhSachNguoiThue = await GetAvailableUsers(),
                DanhSachPhong = await _context.Phongs
                    .Where(p => !p.DaChoThue)
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id, // Giữ nguyên là string
                        Text = $"{p.TenPhong} - {p.GiaTien:N0} đ"
                    }).ToListAsync(),
                NgayBatDau = DateTime.Now
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Them(HopDongViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadDropdownData(vm);
                    return View(vm);
                }

                // Kiểm tra phòng tồn tại và trống
                var phong = await _context.Phongs.FirstOrDefaultAsync(p => p.Id == vm.PhongId); // Sử dụng FirstOrDefaultAsync với điều kiện string
                if (phong == null)
                {
                    ModelState.AddModelError("PhongId", "Phòng không tồn tại");
                    await LoadDropdownData(vm);
                    return View(vm);
                }

                if (phong.DaChoThue)
                {
                    ModelState.AddModelError("PhongId", "Phòng này đã được cho thuê");
                    await LoadDropdownData(vm);
                    return View(vm);
                }

                // Kiểm tra người dùng tồn tại
                var user = await _userManager.FindByIdAsync(vm.UserId);
                if (user == null)
                {
                    ModelState.AddModelError("UserId", "Người dùng không tồn tại");
                    await LoadDropdownData(vm);
                    return View(vm);
                }

                // Tạo hợp đồng
                var hopDong = new HopDong
                {
                    UserId = vm.UserId,
                    PhongId = vm.PhongId, // Sử dụng trực tiếp string
                    NgayBatDau = vm.NgayBatDau,
                    NgayKetThuc = vm.NgayBatDau.AddMonths(6),
                    TienCoc = vm.TienCoc,
                    DaTra = false
                };

                // Sử dụng transaction để đảm bảo toàn vẹn dữ liệu
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // Cập nhật phòng
                        phong.DaChoThue = true;
                        _context.Phongs.Update(phong);

                        // Thêm người thuê nếu chưa có
                        var existingNguoiThue = await _context.NguoiThues.FindAsync(vm.UserId);
                        if (existingNguoiThue == null)
                        {
                            var nguoiThue = new NguoiThue
                            {
                                UserId = vm.UserId,
                                CCCD = vm.CCCD,
                                SDT = vm.SDT
                            };
                            _context.NguoiThues.Add(nguoiThue);
                        }

                        _context.HopDongs.Add(hopDong);
                        await _context.SaveChangesAsync();
                        // Tạo thanh toán cho hợp đồng này
                        var thanhToan = new ThanhToan
                        {
                            UserId = vm.UserId,
                            HopDongId = hopDong.Id,
                            trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan,
                            NgayThanhToan = DateTime.Now,
                            TongTien = phong.GiaTien * 6 + vm.TienCoc, // Tổng tiền phòng (6 tháng) cộng tiền cọc
                            ThangNam = DateTime.Now
                        };

                        _context.ThanhToans.Add(thanhToan);
                        await _context.SaveChangesAsync();

                        // Tạo chi tiết thanh toán cho tiền phòng và tiền cọc
                        _context.ChiTietThanhToans.Add(new ChiTietThanhToan
                        {
                            ThanhToanId = thanhToan.Id,
                            Loai = "TienPhong",
                            MoTa = $"Thanh toán tiền phòng cho phòng {phong.TenPhong}",
                            SoTien = phong.GiaTien * 6 // Tiền phòng 6 tháng
                        });

                        _context.ChiTietThanhToans.Add(new ChiTietThanhToan
                        {
                            ThanhToanId = thanhToan.Id,
                            Loai = "TienCoc",
                            MoTa = "Tiền cọc",
                            SoTien = vm.TienCoc
                        });

                        await _context.SaveChangesAsync();
                        // Tự động đăng ký dịch vụ Điện và Nước cho phòng
                        await DangKyDichVuDienNuoc(vm.PhongId); // Truyền trực tiếp string PhongId

                        // Commit transaction
                        await transaction.CommitAsync();

                        TempData["Success"] = "Thêm hợp đồng thành công.";
                        return RedirectToAction("DanhSach");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Có lỗi xảy ra khi lưu hợp đồng: " + ex.Message);
                        await LoadDropdownData(vm);
                        return View(vm);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi hệ thống: " + ex.Message);
                await LoadDropdownData(vm);
                return View(vm);
            }
        }

        // Hàm tự động đăng ký dịch vụ Điện và Nước
        private async Task DangKyDichVuDienNuoc(string phongId) // Thay đổi tham số thành string
        {
            var dichVuDien = await _context.DichVus.FirstOrDefaultAsync(d => d.TenDichVu == "Điện");
            var dichVuNuoc = await _context.DichVus.FirstOrDefaultAsync(d => d.TenDichVu == "Nước");

            if (dichVuDien != null)
            {
                _context.SuDungDichVus.Add(new SuDungDichVu
                {
                    PhongId = phongId, // Sử dụng string
                    DichVuId = dichVuDien.Id,
                    ChiSoCu = 0,
                    ChiSoMoi = 0,
                    ThangNam = DateTime.Today
                });
            }

            if (dichVuNuoc != null)
            {
                _context.SuDungDichVus.Add(new SuDungDichVu
                {
                    PhongId = phongId, // Sử dụng string
                    DichVuId = dichVuNuoc.Id,
                    ChiSoCu = 0,
                    ChiSoMoi = 0,
                    ThangNam = DateTime.Today
                });
            }

            await _context.SaveChangesAsync();
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
                NgayKetThuc = hopDong.NgayBatDau.AddMonths(6),
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

            if (hopDong.PhongId != vm.PhongId)
            {
                var phongCu = await _context.Phongs.FindAsync(hopDong.PhongId);
                if (phongCu != null)
                {
                    phongCu.DaChoThue = false;
                    _context.Phongs.Update(phongCu);
                }

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
            hopDong.NgayKetThuc = vm.NgayBatDau.AddMonths(6);
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

            _context.HopDongs.Remove(hopDong);

            var phong = await _context.Phongs.FindAsync(phongId);
            if (phong != null)
            {
                phong.DaChoThue = false;
                _context.Phongs.Update(phong);
            }

            await _context.SaveChangesAsync();

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
                .Where(p => !p.DaChoThue || p.Id == vm.PhongId)
                .Select(p => new SelectListItem
                {
                    Value = p.Id,
                    Text = $"{p.TenPhong} - {p.GiaTien:N0} đ",
                    Selected = p.Id == vm.PhongId
                }).ToListAsync();
        }
    }
}
