using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiPhongTro.Controllers
{
    [Authorize(Roles = "ChuTro")]  // Chỉ cho phép Chủ Trọ truy cập
    public class SuCo_ChuTroController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SuCo_ChuTroController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Danh sách sự cố của chủ trọ
        public async Task<IActionResult> DanhSachSuCo()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound("Người dùng không tồn tại");

                // Lấy danh sách các sự cố và phòng
                var suCos = await _context.SuCos
                    .Include(s => s.Phong)  // Lấy thông tin phòng của sự cố
                    .OrderByDescending(s => s.NgayBaoCao)
                    .ToListAsync();

                return View(suCos);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải danh sách sự cố");
            }
        }

        // Chi tiết sự cố
        public async Task<IActionResult> ChiTietSuCo(int id)
        {
            try
            {
                var suCo = await _context.SuCos
                    .Include(s => s.Phong)  // Bao gồm thông tin phòng
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (suCo == null) return NotFound("Sự cố không tồn tại");

                return View(suCo);
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi tải chi tiết sự cố");
            }
        }

        // Cập nhật trạng thái sự cố (Giải quyết sự cố)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatSuCo(int id, SuCo model)
        {
            try
            {
                var suCo = await _context.SuCos.FindAsync(id);
                if (suCo == null) return NotFound("Sự cố không tồn tại");

                suCo.DaGiaiQuyet = model.DaGiaiQuyet;
                suCo.MoTa = model.MoTa;  // Cập nhật mô tả sự cố nếu cần

                _context.Update(suCo);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật sự cố thành công.";
                return RedirectToAction("DanhSachSuCo");
            }
            catch (Exception)
            {
                return View("Error", "Đã xảy ra lỗi khi cập nhật sự cố");
            }
        }
    }
}
