using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        // Xem hợp đồng hiện tại của người thuê
        public async Task<IActionResult> HopDongCuaToi()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound("Người dùng không tồn tại");

                var nguoiThue = await _context.NguoiThues
                    .FirstOrDefaultAsync(n => n.UserId == user.Id);

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

        // Xem danh sách hóa đơn
        public async Task<IActionResult> HoaDonCuaToi()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var nguoiThue = await _context.NguoiThues
                    .FirstOrDefaultAsync(n => n.UserId == user.Id);

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

        // Báo cáo sự cố
        [HttpGet]
        public IActionResult BaoCaoSuCo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BaoCaoSuCo(SuCo model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    if (user == null) return NotFound();

                    var nguoiThue = await _context.NguoiThues
                        .FirstOrDefaultAsync(n => n.UserId == user.Id);

                    if (nguoiThue == null) return NotFound();

                    // Lấy phòng hiện tại của người thuê
                    var phong = await _context.HopDongs
                        .Where(h => h.UserId == nguoiThue.UserId && h.DaTra == false)
                        .Select(h => h.Phong)
                        .FirstOrDefaultAsync();

                    if (phong == null)
                    {
                        ModelState.AddModelError("", "Bạn chưa thuê phòng nào");
                        return View(model);
                    }

                    model.PhongId = phong.Id;
                    model.NgayBaoCao = DateTime.Now;
                    model.DaGiaiQuyet = false;

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

        // Danh sách sự cố đã báo cáo
        public async Task<IActionResult> DanhSachSuCo()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var nguoiThue = await _context.NguoiThues
                    .FirstOrDefaultAsync(n => n.UserId == user.Id);

                if (nguoiThue == null) return NotFound();

                var suCos = await _context.SuCos
                    .Include(s => s.Phong)
                    .Where(s => s.Phong.HopDongs.Any(h => h.UserId == nguoiThue.UserId))
                    .OrderByDescending(s => s.NgayBaoCao)
                    .ToListAsync();

                return View(suCos);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải danh sách sự cố");
            }
        }

        // Xem chi tiết thanh toán
        public async Task<IActionResult> ChiTietThanhToan(int id)
        {
            try
            {
                var thanhToan = await _context.ThanhToans
                    .Include(t => t.ChiTietThanhToans)
                    .Include(t => t.HopDong)
                    .ThenInclude(h => h.Phong)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (thanhToan == null) return NotFound();

                return View(thanhToan);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải chi tiết thanh toán");
            }
        }

        // Trang chủ người thuê
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

                var model = new DashboardNguoiThueViewModel
                {
                    HopDongHienTai = nguoiThue.HopDongs
                        .Where(h => h.DaTra == false)
                        .OrderByDescending(h => h.NgayBatDau)
                        .FirstOrDefault(),
                    SuCos = await _context.SuCos
                        .Where(s => s.Phong.HopDongs.Any(h => h.UserId == nguoiThue.UserId))
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
