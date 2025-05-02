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
    public class NguoiThueController(ApplicationDbContext context, UserManager<IdentityUser> userManager) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<IdentityUser> _userManager = userManager;

        public async Task<IActionResult> HopDongCuaToi()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound("Người dùng không tồn tại");

                var nguoiThue = await _context.NguoiThues.FirstOrDefaultAsync(n => n.UserId == user.Id);
                if (nguoiThue == null) return NotFound("Thông tin người thuê không tồn tại");

                var hopDong = await _context.HopDongs
                    .Include(h => h.Phong)
                    .ThenInclude(p => p.ToaNha)
                    .Where(h => h.UserId == user.Id && h.DaTra == false)
                    .OrderByDescending(h => h.NgayBatDau)
                    .FirstOrDefaultAsync();

                if (hopDong == null) return View("NoContract");

                return View(hopDong);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải hợp đồng");
            }
        }

        public async Task<IActionResult> HoaDonCuaToi()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var nguoiThue = await _context.NguoiThues.FirstOrDefaultAsync(n => n.UserId == user.Id);
                if (nguoiThue == null) return NotFound();

                var hoaDons = await _context.ThanhToans
                    .Include(t => t.HopDong)
                    .ThenInclude(h => h.Phong)
                    .Where(t => t.UserId == nguoiThue.UserId)
                    .OrderByDescending(t => t.NgayThanhToan)
                    .ToListAsync();

                return View(hoaDons);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải hóa đơn");
            }
        }

        public async Task<IActionResult> BaoCaoSuCo()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var nguoiThue = await _context.NguoiThues.FirstOrDefaultAsync(n => n.UserId == user.Id);
            if (nguoiThue == null) return NotFound();

            var phong = await _context.HopDongs
                .Where(h => h.UserId == nguoiThue.UserId && h.DaTra == false)
                .Select(h => h.Phong)
                .FirstOrDefaultAsync();

            if (phong == null)
            {
                TempData["Error"] = "Bạn chưa thuê phòng nào.";
                return RedirectToAction("Index", "Home");
            }

            ViewBag.TenPhong = phong.TenPhong;
            ViewBag.PhongId = phong.Id; // Truyền ID cho form
            return View(new SuCo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaoCaoSuCo(SuCo model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var nguoiThue = await _context.NguoiThues.FirstOrDefaultAsync(n => n.UserId == user.Id);
                if (nguoiThue == null) return NotFound();

                var phong = await _context.HopDongs
                    .Where(h => h.UserId == nguoiThue.UserId && h.DaTra == false)
                    .Select(h => h.Phong)
                    .FirstOrDefaultAsync();

                if (phong == null)
                {
                    ModelState.AddModelError("", "Bạn chưa thuê phòng nào");
                    return View(model);
                }

                ViewBag.TenPhong = phong.TenPhong;
                ViewBag.PhongId = phong.Id;

                // Gán các trường còn lại
                model.NgayBaoCao = DateTime.Now;
                model.DaGiaiQuyet = false;

                if (ModelState.IsValid)
                {
                    _context.SuCos.Add(model);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Đã gửi báo cáo sự cố thành công.";
                    return RedirectToAction("DanhSachSuCo");
                }

                return View(model);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi gửi báo cáo");
            }
        }


        public async Task<IActionResult> DanhSachSuCo()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var nguoiThue = await _context.NguoiThues.FirstOrDefaultAsync(n => n.UserId == user.Id);
                if (nguoiThue == null) return NotFound();

                var hopDongIds = await _context.HopDongs
                    .Where(h => h.UserId == nguoiThue.UserId)
                    .Select(h => h.PhongId)
                    .ToListAsync();

                var suCos = await _context.SuCos
                    .Include(s => s.Phong)
                    .Where(s => hopDongIds.Contains(s.PhongId))
                    .OrderByDescending(s => s.NgayBaoCao)
                    .ToListAsync();

                return View(suCos);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải danh sách sự cố");
            }
        }

        public async Task<IActionResult> DichVuDaDangKy()
        {
            var user = await _userManager.GetUserAsync(User);
            var hopDong = await _context.HopDongs
                .FirstOrDefaultAsync(h => h.UserId == user.Id && !h.DaTra);

            if (hopDong == null)
            {
                TempData["Error"] = "Bạn chưa có hợp đồng thuê phòng đang hoạt động.";
                return RedirectToAction("Index");
            }

            var danhSach = await _context.SuDungDichVus
                .Where(s => s.PhongId == hopDong.PhongId)
                .Include(s => s.DichVu)
                .ToListAsync();

            return View(danhSach);
        }


        [HttpGet]
        public async Task<IActionResult> DangKySuDungDichVu()
        {
            var dichVus = await _context.DichVus
                .Where(d => d.TenDichVu != "Điện" && d.TenDichVu != "Nước") // Lọc bỏ Điện và Nước
                .ToListAsync();

            ViewBag.DichVus = new SelectList(dichVus, "Id", "TenDichVu");

            var user = await _userManager.GetUserAsync(User);
            var hopDong = await _context.HopDongs
                .FirstOrDefaultAsync(h => h.UserId == user.Id && !h.DaTra);

            if (hopDong == null)
            {
                TempData["Error"] = "Bạn chưa có hợp đồng thuê phòng.";
                return RedirectToAction("Index");
            }

            var model = new SuDungDichVu
            {
                PhongId = hopDong.PhongId,
                ChiSoCu = 0,
                ChiSoMoi = 0,
                ThangNam = DateTime.Today
            };

            return View(model);
        }

        //[HttpPost]
        //public async Task<IActionResult> DangKySuDungDichVu(SuDungDichVu model)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    var hopDong = await _context.HopDongs
        //        .FirstOrDefaultAsync(h => h.UserId == user.Id && !h.DaTra);

        //    if (hopDong == null)
        //    {
        //        TempData["Error"] = "Bạn chưa có hợp đồng thuê phòng.";
        //        return RedirectToAction("Index");
        //    }

        //    model.PhongId = hopDong.PhongId;
        //    model.ChiSoCu = 0;
        //    model.ChiSoMoi = 0;

        //    // ✅ Kiểm tra đã đăng ký dịch vụ chưa
        //    bool daDangKy = await _context.SuDungDichVus
        //        .AnyAsync(s => s.PhongId == model.PhongId && s.DichVuId == model.DichVuId);

        //    if (daDangKy)
        //    {
        //        TempData["Error"] = "Dịch vụ này đã được đăng ký.";
        //        return RedirectToAction("DichVuDaDangKy");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        var dichVus = await _context.DichVus
        //            .Where(d => d.TenDichVu != "Điện" && d.TenDichVu != "Nước")
        //            .ToListAsync();
        //        ViewBag.DichVus = new SelectList(dichVus, "Id", "TenDichVu");

        //        return View(model);
        //    }

        //    _context.SuDungDichVus.Add(model);
        //    await _context.SaveChangesAsync();

        //    TempData["Success"] = "Đăng ký dịch vụ thành công.";
        //    return RedirectToAction("DichVuDaDangKy");
        //}

        [HttpPost]
        public async Task<IActionResult> DangKySuDungDichVu(SuDungDichVu model)
        {
            var user = await _userManager.GetUserAsync(User);
            var hopDong = await _context.HopDongs
                .FirstOrDefaultAsync(h => h.UserId == user.Id && !h.DaTra);

            if (hopDong == null)
            {
                TempData["Error"] = "Bạn chưa có hợp đồng thuê phòng.";
                return RedirectToAction("Index");
            }

            model.PhongId = hopDong.PhongId;
            model.ChiSoCu = 0;
            model.ChiSoMoi = 0;
            model.ThangNam = DateTime.Today;

            if (!ModelState.IsValid)
            {
                var dichVus = await _context.DichVus
                    .Where(d => d.TenDichVu != "Điện" && d.TenDichVu != "Nước")
                    .ToListAsync();
                ViewBag.DichVus = new SelectList(dichVus, "Id", "TenDichVu");

                return View(model);
            }

            bool daDangKy = await _context.SuDungDichVus
                .AnyAsync(s => s.PhongId == model.PhongId
                    && s.DichVuId == model.DichVuId
                    && s.ThangNam.Month == model.ThangNam.Month
                    && s.ThangNam.Year == model.ThangNam.Year);

            if (daDangKy)
            {
                TempData["Error"] = "Dịch vụ này đã được đăng ký trong tháng.";
                return RedirectToAction("DichVuDaDangKy");
            }

            var dichVu = await _context.DichVus.FindAsync(model.DichVuId);
            if (dichVu == null)
            {
                TempData["Error"] = "Dịch vụ không tồn tại.";
                return RedirectToAction("DichVuDaDangKy");
            }

            // Thêm bản ghi sử dụng dịch vụ
            _context.SuDungDichVus.Add(model);
            await _context.SaveChangesAsync();

            // Tạo hóa đơn trả trước cho dịch vụ nếu KHÔNG phải là điện/nước
            if (!dichVu.TenDichVu.ToLower().Contains("điện") && !dichVu.TenDichVu.ToLower().Contains("nước"))
            {
                var thanhToan = new ThanhToan
                {
                    UserId = user.Id,
                    HopDongId = hopDong.Id,
                    NgayThanhToan = DateTime.Now,
                    ThangNam = model.ThangNam,
                    trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan,
                    TongTien = dichVu.DonGia
                };

                _context.ThanhToans.Add(thanhToan);
                await _context.SaveChangesAsync();

                var chiTiet = new ChiTietThanhToan
                {
                    ThanhToanId = thanhToan.Id,
                    Loai = "DichVu",
                    MoTa = $"Trả trước dịch vụ: {dichVu.TenDichVu} tháng {model.ThangNam:MM/yyyy}",
                    SoTien = dichVu.DonGia,
                    SuDungDichVuId = model.Id
                };

                _context.ChiTietThanhToans.Add(chiTiet);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Đăng ký dịch vụ thành công và đã tạo hóa đơn.";
            return RedirectToAction("DichVuDaDangKy");
        }

        [HttpPost]
        public async Task<IActionResult> HuyDangKyDichVu(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var hopDong = await _context.HopDongs
                .FirstOrDefaultAsync(h => h.UserId == user.Id && !h.DaTra);

            var suDung = await _context.SuDungDichVus
                .FirstOrDefaultAsync(s => s.Id == id && s.PhongId == hopDong.PhongId);

            if (suDung == null)
            {
                TempData["Error"] = "Không tìm thấy dịch vụ cần hủy.";
                return RedirectToAction("DichVuDaDangKy");
            }

            _context.SuDungDichVus.Remove(suDung);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Hủy đăng ký dịch vụ thành công.";
            return RedirectToAction("DichVuDaDangKy");
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var nguoiThue = await _context.NguoiThues
                    .Include(n => n.HopDongs)
                    .ThenInclude(h => h.Phong)
                    .FirstOrDefaultAsync(n => n.UserId == user.Id);

                if (nguoiThue == null) return NotFound();

                var phongIds = nguoiThue.HopDongs.Where(h => !h.DaTra).Select(h => h.PhongId).ToList();

                var model = new DashboardNguoiThueViewModel
                {
                    HopDongHienTai = nguoiThue.HopDongs
                        .Where(h => h.DaTra == false)
                        .OrderByDescending(h => h.NgayBatDau)
                        .FirstOrDefault(),
                    SuCos = await _context.SuCos
                        .Where(s => phongIds.Contains(s.PhongId))
                        .OrderByDescending(s => s.NgayBaoCao)
                        .Take(5)
                        .ToListAsync(),
                    HoaDons = await _context.ThanhToans
                        .Where(t => t.UserId == nguoiThue.UserId)
                        .OrderByDescending(t => t.NgayThanhToan)
                        .Take(5)
                        .ToListAsync()
                };

                return View(model);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải trang chủ");
            }
        }
    }
}