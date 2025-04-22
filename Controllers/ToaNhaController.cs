using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;
using Microsoft.Extensions.Logging;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "ChuTro")]
    public class ToaNhaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ToaNhaController> _logger;

        public ToaNhaController(ApplicationDbContext context, ILogger<ToaNhaController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var toaNhas = await _context.ToaNhas.ToListAsync();
                return View(toaNhas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tải danh sách tòa nhà");
                TempData["ErrorMessage"] = "Không thể tải danh sách tòa nhà.";
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TenToa")] ToaNha model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (await _context.ToaNhas.AnyAsync(t => t.Id == model.Id))
                    {
                        ModelState.AddModelError("Id", "Mã tòa nhà đã tồn tại.");
                        return View(model);
                    }

                    _context.ToaNhas.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm tòa nhà thành công.";
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm tòa nhà");
                TempData["ErrorMessage"] = "Không thể thêm tòa nhà.";
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var model = await _context.ToaNhas.FindAsync(id);
                if (model == null) return NotFound();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tải tòa nhà {id} để sửa");
                TempData["ErrorMessage"] = "Không thể tải dữ liệu để chỉnh sửa.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,TenToa")] ToaNha model)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật tòa nhà thành công.";
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ToaNhas.Any(e => e.Id == model.Id)) return NotFound();
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật tòa nhà {id}");
                TempData["ErrorMessage"] = "Không thể cập nhật tòa nhà.";
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var model = await _context.ToaNhas.FindAsync(id);
                if (model == null) return NotFound();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tải tòa nhà {id} để xoá");
                TempData["ErrorMessage"] = "Không thể tải thông tin tòa nhà.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var model = await _context.ToaNhas.FindAsync(id);
                if (model == null) return NotFound();
                _context.ToaNhas.Remove(model);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa tòa nhà thành công.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xoá tòa nhà {id}");
                TempData["ErrorMessage"] = "Không thể xoá tòa nhà.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
