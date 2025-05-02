using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "ChuTro")]
    public class SuDungDichVuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuDungDichVuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SuDungDichVu
        public async Task<IActionResult> Index()
        {
            var suDungDichVus = await _context.SuDungDichVus
                .Include(s => s.Phong)
                .Include(s => s.DichVu)
                .ToListAsync();
            return View(suDungDichVus);
        }

        // GET: SuDungDichVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var suDungDichVu = await _context.SuDungDichVus
                .Include(s => s.DichVu)
                .Include(s => s.Phong)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (suDungDichVu == null)
                return NotFound();

            // Truyền tên dịch vụ qua ViewBag để kiểm tra dịch vụ nào
            ViewBag.TenDichVu = suDungDichVu.DichVu?.TenDichVu;

            ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong", suDungDichVu.PhongId);
            ViewData["DichVuId"] = new SelectList(_context.DichVus, "Id", "TenDichVu", suDungDichVu.DichVuId);

            return View(suDungDichVu);
        }


        // POST: SuDungDichVu/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,ChiSoCu,ChiSoMoi,ThangNam,DichVuId,PhongId")] SuDungDichVu suDungDichVu)
        //{
        //    if (id != suDungDichVu.Id)
        //        return NotFound();

        //    var dichVu = await _context.DichVus.FindAsync(suDungDichVu.DichVuId);
        //    if (dichVu == null)
        //        return NotFound();

        //    bool laDichVuCanChiSo = dichVu.TenDichVu.Contains("điện", StringComparison.OrdinalIgnoreCase)
        //                          || dichVu.TenDichVu.Contains("nước", StringComparison.OrdinalIgnoreCase);

        //    if (laDichVuCanChiSo)
        //    {
        //        if (suDungDichVu.ChiSoMoi < suDungDichVu.ChiSoCu)
        //        {
        //            ModelState.AddModelError("ChiSoMoi", "Chỉ số mới phải lớn hơn hoặc bằng chỉ số cũ");
        //        }
        //    }
        //    else
        //    {
        //        suDungDichVu.ChiSoCu = 0;
        //        suDungDichVu.ChiSoMoi = 0;
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(suDungDichVu);
        //            await _context.SaveChangesAsync();

        //            var suDung = await _context.SuDungDichVus
        //                .Include(s => s.DichVu)
        //                .Include(s => s.Phong)
        //                    .ThenInclude(p => p.HopDongs.Where(h => h.NgayKetThuc == null))
        //                .FirstOrDefaultAsync(s => s.Id == suDungDichVu.Id);

        //            if (suDung?.DichVu == null || suDung.Phong == null)
        //                return RedirectToAction(nameof(Index));

        //            var hopDong = suDung.Phong.HopDongs.FirstOrDefault();
        //            if (hopDong == null)
        //                return RedirectToAction(nameof(Index));

        //            decimal tienDichVu = 0;
        //            string moTa = "";

        //            if (laDichVuCanChiSo)
        //            {
        //                var soLuong = suDung.ChiSoMoi - suDung.ChiSoCu;
        //                tienDichVu = soLuong * suDung.DichVu.DonGia;
        //                moTa = $"{suDung.DichVu.TenDichVu} (Từ {suDung.ChiSoCu} đến {suDung.ChiSoMoi})";
        //            }
        //            else
        //            {
        //                tienDichVu = suDung.DichVu.DonGia;
        //                moTa = $"{suDung.DichVu.TenDichVu} tháng {suDung.ThangNam:MM/yyyy}";
        //            }

        //            var thanhToan = await _context.ThanhToans
        //                .Include(t => t.ChiTietThanhToans)
        //                .FirstOrDefaultAsync(t =>
        //                    t.HopDongId == hopDong.Id &&
        //                    t.ThangNam.Month == suDung.ThangNam.Month &&
        //                    t.ThangNam.Year == suDung.ThangNam.Year);

        //            if (thanhToan == null)
        //            {
        //                thanhToan = new ThanhToan
        //                {
        //                    HopDongId = hopDong.Id,
        //                    ThangNam = new DateTime(suDung.ThangNam.Year, suDung.ThangNam.Month, 1),
        //                    trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan,
        //                    ChiTietThanhToans = new List<ChiTietThanhToan>()
        //                };
        //                _context.ThanhToans.Add(thanhToan);
        //            }

        //            var chiTiet = thanhToan.ChiTietThanhToans
        //                .FirstOrDefault(ct => ct.SuDungDichVuId == suDung.Id);

        //            if (chiTiet == null)
        //            {
        //                chiTiet = new ChiTietThanhToan
        //                {
        //                    SuDungDichVuId = suDung.Id,
        //                    MoTa = moTa,
        //                    SoTien = tienDichVu
        //                };
        //                thanhToan.ChiTietThanhToans.Add(chiTiet);
        //            }
        //            else
        //            {
        //                chiTiet.MoTa = moTa;
        //                chiTiet.SoTien = tienDichVu;
        //            }

        //            thanhToan.TongTien = thanhToan.ChiTietThanhToans.Sum(ct => ct.SoTien);

        //            await _context.SaveChangesAsync();
        //            TempData["Message"] = "Cập nhật chỉ số và hóa đơn dịch vụ thành công.";
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!SuDungDichVuExists(suDungDichVu.Id))
        //                return NotFound();
        //            else
        //                throw;
        //        }

        //        return RedirectToAction(nameof(Index));
        //    }

        //    // Nếu ModelState không hợp lệ, truyền lại ViewBag và TenDichVu
        //    ViewBag.TenDichVu = dichVu?.TenDichVu;
        //    ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong", suDungDichVu.PhongId);
        //    ViewData["DichVuId"] = new SelectList(_context.DichVus, "Id", "TenDichVu", suDungDichVu.DichVuId);

        //    return View(suDungDichVu);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChiSoCu,ChiSoMoi,ThangNam,DichVuId,PhongId")] SuDungDichVu suDungDichVu)
        {
            if (id != suDungDichVu.Id)
                return NotFound();

            var dichVu = await _context.DichVus.FindAsync(suDungDichVu.DichVuId);
            if (dichVu == null)
                return NotFound();

            bool laDichVuCanChiSo = dichVu.TenDichVu.Contains("điện", StringComparison.OrdinalIgnoreCase)
                                  || dichVu.TenDichVu.Contains("nước", StringComparison.OrdinalIgnoreCase);

            // Kiểm tra chỉ số
            if (laDichVuCanChiSo)
            {
                if (suDungDichVu.ChiSoMoi < suDungDichVu.ChiSoCu)
                {
                    ModelState.AddModelError("ChiSoMoi", "Chỉ số mới phải lớn hơn hoặc bằng chỉ số cũ");
                }
            }
            else
            {
                suDungDichVu.ChiSoCu = 0;
                suDungDichVu.ChiSoMoi = 0;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin SuDungDichVu
                    _context.Update(suDungDichVu);
                    await _context.SaveChangesAsync();

                    // Tìm SuDungDichVu vừa cập nhật
                    var suDung = await _context.SuDungDichVus
                        .Include(s => s.DichVu)
                        .Include(s => s.Phong)
                            .ThenInclude(p => p.HopDongs.Where(h => h.NgayKetThuc == null))
                        .FirstOrDefaultAsync(s => s.Id == suDungDichVu.Id);

                    if (suDung?.DichVu == null || suDung.Phong == null)
                        return RedirectToAction(nameof(Index));

                    var hopDong = suDung.Phong.HopDongs.FirstOrDefault();
                    if (hopDong == null)
                        return RedirectToAction(nameof(Index));

                    decimal tienDichVu = 0;
                    string moTa = "";

                    // Tính toán tiền dịch vụ và mô tả
                    if (laDichVuCanChiSo)
                    {
                        var soLuong = suDung.ChiSoMoi - suDung.ChiSoCu;
                        tienDichVu = soLuong * suDung.DichVu.DonGia;
                        moTa = $"{suDung.DichVu.TenDichVu} (Từ {suDung.ChiSoCu} đến {suDung.ChiSoMoi})";
                    }
                    else
                    {
                        tienDichVu = suDung.DichVu.DonGia;
                        moTa = $"{suDung.DichVu.TenDichVu} tháng {suDung.ThangNam:MM/yyyy}";
                    }

                    // Kiểm tra xem có hóa đơn cho tháng này chưa
                    var thanhToan = await _context.ThanhToans
                        .Include(t => t.ChiTietThanhToans)
                        .FirstOrDefaultAsync(t =>
                            t.HopDongId == hopDong.Id &&
                            t.ThangNam.Month == suDung.ThangNam.Month &&
                            t.ThangNam.Year == suDung.ThangNam.Year);

                    if (thanhToan == null)
                    {
                        // Nếu chưa có hóa đơn, tạo mới hóa đơn
                        thanhToan = new ThanhToan
                        {
                            HopDongId = hopDong.Id,
                            ThangNam = new DateTime(suDung.ThangNam.Year, suDung.ThangNam.Month, 1),
                            trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan,
                            ChiTietThanhToans = new List<ChiTietThanhToan>()
                        };
                        _context.ThanhToans.Add(thanhToan);
                    }

                    // Thêm hoặc cập nhật chi tiết thanh toán vào hóa đơn
                    var chiTiet = thanhToan.ChiTietThanhToans
                        .FirstOrDefault(ct => ct.SuDungDichVuId == suDung.Id);

                    if (chiTiet == null)
                    {
                        chiTiet = new ChiTietThanhToan
                        {
                            SuDungDichVuId = suDung.Id,
                            MoTa = moTa,
                            SoTien = tienDichVu,
                            Loai = "Dịch vụ" // Gán loại dịch vụ mặc định
                        };
                        thanhToan.ChiTietThanhToans.Add(chiTiet);
                    }
                    else
                    {
                        chiTiet.MoTa = moTa;
                        chiTiet.SoTien = tienDichVu;
                    }

                    // Cập nhật tổng tiền hóa đơn
                    thanhToan.TongTien = thanhToan.ChiTietThanhToans.Sum(ct => ct.SoTien);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật chỉ số và hóa đơn dịch vụ thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuDungDichVuExists(suDungDichVu.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, truyền lại ViewBag và TenDichVu
            ViewBag.TenDichVu = dichVu?.TenDichVu;
            ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong", suDungDichVu.PhongId);
            ViewData["DichVuId"] = new SelectList(_context.DichVus, "Id", "TenDichVu", suDungDichVu.DichVuId);

            return View(suDungDichVu);
        }


        private bool SuDungDichVuExists(int id)
        {
            return _context.SuDungDichVus.Any(e => e.Id == id);
        }
    }
}
