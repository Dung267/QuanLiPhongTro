using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "ChuTro")]
    public class PhongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhongController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string trangThai = "")
        {
            var query = _context.Phongs.Include(p => p.ToaNha).AsQueryable();

            if (trangThai == "trong")
                query = query.Where(p => !p.DaChoThue);
            else if (trangThai == "thue")
                query = query.Where(p => p.DaChoThue);

            return View(await query.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.ToaNhaList = new SelectList(_context.ToaNhas, "Id", "TenToa");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenPhong,SoNguoiToiDa,GiaTien,DaChoThue,ToaNhaId")] Phong model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Phongs.AnyAsync(p => p.Id == model.Id))
                {
                    ModelState.AddModelError("Id", "Mã phòng đã tồn tại.");
                }
                else
                {
                    _context.Phongs.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm phòng thành công.";
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.ToaNhaList = new SelectList(_context.ToaNhas, "Id", "TenToa", model.ToaNhaId);
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var model = await _context.Phongs.FindAsync(id);
            if (model == null) return NotFound();

            ViewBag.ToaNhaList = new SelectList(_context.ToaNhas, "Id", "TenToa", model.ToaNhaId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Phong model)
        {
            if (id != model.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật phòng thành công.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ToaNhaList = new SelectList(_context.ToaNhas, "Id", "TenToa", model.ToaNhaId);
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var model = await _context.Phongs.Include(p => p.ToaNha).FirstOrDefaultAsync(p => p.Id == id);
            if (model == null) return NotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var model = await _context.Phongs.FindAsync(id);
            if (model == null) return NotFound();

            _context.Phongs.Remove(model);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa phòng thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
