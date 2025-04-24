using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    //[Authorize(Roles = "ChuTro")]
    //public class ThanhToanController : Controller
    //{
    //    private readonly ApplicationDbContext _context;
    //    private readonly UserManager<IdentityUser> _userManager;

    //    public ThanhToanController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    //    {
    //        _context = context;
    //        _userManager = userManager;
    //    }

    //    // ==================== DANH SÁCH THANH TOÁN ====================
    //    public async Task<IActionResult> Index()
    //    {
    //        var list = await _context.ThanhToans
    //            .Include(t => t.User)
    //            .Include(t => t.HopDong)
    //            .ToListAsync();

    //        return View(list);
    //    }

    //    // ==================== CHI TIẾT ====================
    //    public async Task<IActionResult> ChiTiet(int id)
    //    {
    //        var thanhToan = await _context.ThanhToans
    //            .Include(t => t.User)
    //            .Include(t => t.HopDong)
    //            .Include(t => t.ChiTietThanhToans)
    //            .FirstOrDefaultAsync(t => t.Id == id);

    //        if (thanhToan == null) return NotFound();

    //        return View(thanhToan);
    //    }

    //    // ==================== THÊM MỚI ====================
    //    [HttpGet]
    //    public async Task<IActionResult> Them()
    //    {
    //        ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
    //        ViewBag.HopDongs = new SelectList(await _context.HopDongs.ToListAsync(), "Id", "Id");
    //        return View();
    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Them(ThanhToan model)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            ViewBag.Users = new SelectList(await _context.Users.ToListAsync(), "Id", "UserName");
    //            ViewBag.HopDongs = new SelectList(await _context.HopDongs.ToListAsync(), "Id", "Id");
    //            return View(model);
    //        }

    //        model.NgayThanhToan = DateTime.Now;
    //        _context.ThanhToans.Add(model);
    //        await _context.SaveChangesAsync();

    //        TempData["Success"] = "Thêm thanh toán thành công.";
    //        return RedirectToAction("Index");
    //    }

    //    // ==================== XÓA ====================
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Xoa(int id)
    //    {
    //        var thanhToan = await _context.ThanhToans
    //            .Include(t => t.ChiTietThanhToans)
    //            .FirstOrDefaultAsync(t => t.Id == id);

    //        if (thanhToan == null) return NotFound();

    //        _context.ChiTietThanhToans.RemoveRange(thanhToan.ChiTietThanhToans);
    //        _context.ThanhToans.Remove(thanhToan);
    //        await _context.SaveChangesAsync();

    //        TempData["Success"] = "Xóa thanh toán thành công.";
    //        return RedirectToAction("Index");
    //    }
    //}

    [Authorize(Roles = "ChuTro")]
    public class ThanhToanController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ThanhToanController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(TrangThaiThanhToan? trangThai, DateTime? tuNgay, DateTime? denNgay)
        {
            var query = _context.ThanhToans
                .Include(t => t.User)
                .Include(t => t.HopDong)
                .AsQueryable();

            if (trangThai.HasValue)
                query = query.Where(t => t.trangThaiThanhToan == trangThai);

            if (tuNgay.HasValue)
                query = query.Where(t => t.NgayThanhToan >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(t => t.NgayThanhToan <= denNgay.Value);

            var vm = new ThanhToanViewModel
            {
                TrangThai = trangThai,
                TuNgay = tuNgay,
                DenNgay = denNgay,
                DanhSachThanhToan = await query.OrderByDescending(t => t.NgayThanhToan).ToListAsync()
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var thanhToan = await _context.ThanhToans
                .Include(t => t.HopDong)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (thanhToan == null) return NotFound();

            string tenNguoiThue = "Không rõ";
            if (thanhToan.HopDong?.UserId != null)
            {
                var user = await _userManager.FindByIdAsync(thanhToan.HopDong.UserId);
                tenNguoiThue = user?.UserName ?? "Không rõ"; 
            }

            ViewBag.TenNguoiThue = tenNguoiThue;
            return View(thanhToan);
        }


        public IActionResult Create()
        {
            ViewBag.HopDongs = new SelectList(_context.HopDongs.Include(h => h.UserId), "Id", "TenNguoiThue");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ThanhToan model)
        {
            if (ModelState.IsValid)
            {
                model.trangThaiThanhToan = TrangThaiThanhToan.ChuaThanhToan;
                model.NgayThanhToan = DateTime.Now;

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> XacNhanThanhToan(int id)
        {
            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan == null) return NotFound();

            thanhToan.trangThaiThanhToan = TrangThaiThanhToan.DaThanhToan;
            thanhToan.NgayThanhToan = DateTime.Now;

            await _context.SaveChangesAsync();
            TempData["Success"] = "Xác nhận thanh toán thành công!";
            return RedirectToAction("Index");
        }



    }

}