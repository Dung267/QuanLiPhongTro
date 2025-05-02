using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    [Route("SuCo_QL")]
    public class SuCo_QLController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuCo_QLController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var suCoList = await _context.SuCos
                .Select(s => new SuCoViewModel
                {
                    Id = s.Id,
                    MoTa = s.MoTa,
                    NgayBaoCao = s.NgayBaoCao,
                    PhongId = s.PhongId,
                    DaGiaiQuyet = s.DaGiaiQuyet
                })
                .ToListAsync();

            return View(suCoList);
        }

        [HttpPost("CapNhatTrangThai")]
        public async Task<IActionResult> CapNhatTrangThai(int id, bool daGiaiQuyet)
        {
            var suCo = await _context.SuCos.FindAsync(id);
            if (suCo == null) return NotFound();

            suCo.DaGiaiQuyet = daGiaiQuyet;
            _context.Update(suCo);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
