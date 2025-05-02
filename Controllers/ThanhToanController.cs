using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "User")]
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ThanhToanController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            var thanhToans = await _context.ThanhToans
                .Where(t => t.UserId == userId)
                .Include(t => t.HopDong)
                    .ThenInclude(h => h.Phong)
                .Include(t => t.ChiTietThanhToans)
                .OrderByDescending(t => t.ThangNam)
                .ToListAsync();

            return View(thanhToans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var thanhToan = await _context.ThanhToans
                .Include(t => t.HopDong)
                    .ThenInclude(h => h.Phong)
                .Include(t => t.ChiTietThanhToans)
                    .ThenInclude(ct => ct.SuDungDichVu)
                        .ThenInclude(s => s.DichVu)
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (thanhToan == null)
                return NotFound();

            return View(thanhToan);
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToan(int id)
        {
            var userId = _userManager.GetUserId(User);
            var thanhToan = await _context.ThanhToans.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (thanhToan == null)
                return NotFound();

            thanhToan.trangThaiThanhToan = TrangThaiThanhToan.DaThanhToan;
            thanhToan.NgayThanhToan = DateTime.Now;
            _context.Update(thanhToan);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thanh toán hóa đơn thành công";
            return RedirectToAction(nameof(Details), new { id });
        }

        private async Task<bool> KiemTraHopDongThuocNguoiDung(int hopDongId, string userId)
        {
            return await _context.HopDongs.AnyAsync(h => h.Id == hopDongId && h.UserId == userId);
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToanTienPhong(int hopDongId)
        {
            var userId = _userManager.GetUserId(User);
            if (!await KiemTraHopDongThuocNguoiDung(hopDongId, userId))
                return Forbid();
            var hopDong = await _context.HopDongs
                .Include(h => h.Phong)
                .FirstOrDefaultAsync(h => h.Id == hopDongId);

            if (hopDong == null)
            {
                return NotFound();
            }

            // Tính số tháng thuê dựa trên thời gian bắt đầu và kết thúc
            var thoiGianThue = (hopDong.NgayKetThuc - hopDong.NgayBatDau).Days / 30;

            // Nếu thời gian thuê quá ngắn, có thể tính theo số ngày
            if (thoiGianThue < 1)
            {
                thoiGianThue = 1;
            }

            // Tính tiền phòng
            var soTienPhong = hopDong.Phong.GiaTien * thoiGianThue;

            var thanhToan = new ThanhToan
            {
                UserId = hopDong.UserId,
                HopDongId = hopDong.Id,
                trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan,
                NgayThanhToan = DateTime.Now,
                TongTien = soTienPhong,
                ThangNam = DateTime.Now
            };

            _context.ThanhToans.Add(thanhToan);
            await _context.SaveChangesAsync();

            // Thêm chi tiết thanh toán cho tiền phòng
            _context.ChiTietThanhToans.Add(new ChiTietThanhToan
            {
                ThanhToanId = thanhToan.Id,
                Loai = "TienPhong",
                MoTa = $"Thanh toán tiền phòng cho phòng {hopDong.Phong.TenPhong}",
                SoTien = soTienPhong
            });

            await _context.SaveChangesAsync();

            TempData["Message"] = "Thanh toán tiền phòng thành công.";
            return RedirectToAction(nameof(Details), new { id = thanhToan.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToanDichVu(int hopDongId)
        {
            var userId = _userManager.GetUserId(User);
            if (!await KiemTraHopDongThuocNguoiDung(hopDongId, userId))
                return Forbid();
            var hopDong = await _context.HopDongs
                .Include(h => h.Phong)
                .FirstOrDefaultAsync(h => h.Id == hopDongId);

            if (hopDong == null)
            {
                return NotFound();
            }

            var thangNamHienTai = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Lấy các dịch vụ được sử dụng trong phòng tương ứng với hợp đồng
            var suDungDichVus = await _context.SuDungDichVus
                .Include(s => s.DichVu)
                .Where(s => s.PhongId == hopDong.PhongId && s.ThangNam.Year == thangNamHienTai.Year && s.ThangNam.Month == thangNamHienTai.Month)
                .ToListAsync();

            if (!suDungDichVus.Any())
            {
                TempData["Error"] = "Không có dịch vụ nào được sử dụng trong tháng này.";
                return RedirectToAction("Index");
            }

            // Tính tổng tiền dịch vụ
            decimal tongTienDichVu = suDungDichVus.Sum(s => s.DichVu.DonGia);

            var thanhToan = new ThanhToan
            {
                UserId = hopDong.UserId,
                HopDongId = hopDong.Id,
                trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan,
                NgayThanhToan = DateTime.Now,
                TongTien = tongTienDichVu,
                ThangNam = DateTime.Now
            };

            _context.ThanhToans.Add(thanhToan);
            await _context.SaveChangesAsync();

            // Thêm chi tiết từng dịch vụ
            foreach (var sd in suDungDichVus)
            {
                _context.ChiTietThanhToans.Add(new ChiTietThanhToan
                {
                    ThanhToanId = thanhToan.Id,
                    SuDungDichVuId = sd.Id,
                    Loai = "DichVu",
                    MoTa = $"Dịch vụ: {sd.DichVu.TenDichVu}",
                    SoTien =sd.DichVu.DonGia
                });
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Thanh toán dịch vụ thành công.";
            return RedirectToAction(nameof(Details), new { id = thanhToan.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ThanhToanDichVuTraTruoc(int hopDongId)
        {
            var userId = _userManager.GetUserId(User);
            if (!await KiemTraHopDongThuocNguoiDung(hopDongId, userId))
                return Forbid();
            var hopDong = await _context.HopDongs
                .Include(h => h.Phong)
                .FirstOrDefaultAsync(h => h.Id == hopDongId);

            if (hopDong == null)
            {
                return NotFound();
            }

            var thangNamHienTai = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Lấy danh sách dịch vụ đã đăng ký (không bao gồm điện/nước)
            var dichVusDangKy = await _context.SuDungDichVus
                .Include(s => s.DichVu)
                .Where(s => s.PhongId == hopDong.PhongId &&
                            s.ThangNam.Year == thangNamHienTai.Year &&
                            s.ThangNam.Month == thangNamHienTai.Month &&
                            s.DichVu.TenDichVu != "Điện" &&
                            s.DichVu.TenDichVu != "Nước")
                .ToListAsync();

            if (!dichVusDangKy.Any())
            {
                TempData["Error"] = "Không có dịch vụ đăng ký trả trước trong tháng này.";
                return RedirectToAction("Index");
            }

            // Kiểm tra đã thanh toán dịch vụ nào chưa (tránh trùng)
            var daThanhToanIds = await _context.ChiTietThanhToans
                .Where(ct => ct.ThanhToan.HopDongId == hopDongId &&
                             ct.ThanhToan.ThangNam.Year == thangNamHienTai.Year &&
                             ct.ThanhToan.ThangNam.Month == thangNamHienTai.Month &&
                             ct.Loai == "DichVu")
                .Select(ct => ct.SuDungDichVuId)
                .ToListAsync();

            var dichVuChuaThanhToan = dichVusDangKy.Where(s => !daThanhToanIds.Contains(s.Id)).ToList();

            if (!dichVuChuaThanhToan.Any())
            {
                TempData["Error"] = "Tất cả dịch vụ đã được thanh toán trong tháng này.";
                return RedirectToAction("Index");
            }

            var tongTien = dichVuChuaThanhToan.Sum(s => s.DichVu.DonGia);

            var thanhToan = new ThanhToan
            {
                UserId = hopDong.UserId,
                HopDongId = hopDong.Id,
                trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan,
                NgayThanhToan = DateTime.Now,
                TongTien = tongTien,
                ThangNam = thangNamHienTai
            };

            _context.ThanhToans.Add(thanhToan);
            await _context.SaveChangesAsync();

            foreach (var sd in dichVuChuaThanhToan)
            {
                _context.ChiTietThanhToans.Add(new ChiTietThanhToan
                {
                    ThanhToanId = thanhToan.Id,
                    SuDungDichVuId = sd.Id,
                    Loai = "DichVu",
                    MoTa = $"Trả trước dịch vụ: {sd.DichVu.TenDichVu}",
                    SoTien = sd.DichVu.DonGia
                });
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = "Thanh toán dịch vụ trả trước thành công.";
            return RedirectToAction(nameof(Details), new { id = thanhToan.Id });
        }


    }

}