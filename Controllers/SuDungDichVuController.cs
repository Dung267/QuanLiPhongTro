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
        public async Task<IActionResult> Index(int? phongId)
        {
            var query = _context.SuDungDichVus
                .Include(s => s.DichVu)
                .Include(s => s.Phong)
                .AsQueryable();

            if (phongId.HasValue)
            {
                query = query.Where(s => s.PhongId == phongId.Value);
            }

            return View(await query.ToListAsync());
        }

        // GET: SuDungDichVu/Create
        public IActionResult Create()
        {
            ViewData["DichVuId"] = new SelectList(_context.DichVus, "Id", "TenDichVu");
            ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong");
            return View();
        }

        // POST: SuDungDichVu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChiSoCu,ChiSoMoi,ThangNam,DichVuId,PhongId")] SuDungDichVu suDungDichVu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suDungDichVu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DichVuId"] = new SelectList(_context.DichVus, "Id", "TenDichVu", suDungDichVu.DichVuId);
            ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong", suDungDichVu.PhongId);
            return View(suDungDichVu);
        }

        // GET: SuDungDichVu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suDungDichVu = await _context.SuDungDichVus.FindAsync(id);
            if (suDungDichVu == null)
            {
                return NotFound();
            }
            ViewData["DichVuId"] = new SelectList(_context.DichVus, "Id", "TenDichVu", suDungDichVu.DichVuId);
            ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong", suDungDichVu.PhongId);
            return View(suDungDichVu);
        }

        // POST: SuDungDichVu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChiSoCu,ChiSoMoi,ThangNam,DichVuId,PhongId")] SuDungDichVu suDungDichVu)
        {
            if (id != suDungDichVu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suDungDichVu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuDungDichVuExists(suDungDichVu.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DichVuId"] = new SelectList(_context.DichVus, "Id", "TenDichVu", suDungDichVu.DichVuId);
            ViewData["PhongId"] = new SelectList(_context.Phongs, "Id", "TenPhong", suDungDichVu.PhongId);
            return View(suDungDichVu);
        }

        // GET: SuDungDichVu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suDungDichVu = await _context.SuDungDichVus
                .Include(s => s.DichVu)
                .Include(s => s.Phong)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suDungDichVu == null)
            {
                return NotFound();
            }

            return View(suDungDichVu);
        }

        // POST: SuDungDichVu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suDungDichVu = await _context.SuDungDichVus.FindAsync(id);
            _context.SuDungDichVus.Remove(suDungDichVu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuDungDichVuExists(int id)
        {
            return _context.SuDungDichVus.Any(e => e.Id == id);
        }
    }
}
