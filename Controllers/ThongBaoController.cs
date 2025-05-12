using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;

namespace QuanLiPhongTro.Controllers
{
    public class ThongBaoController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public ThongBaoController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult DanhSach()
        {
            var ds = ThongBaoData.DanhSachThongBao
                .OrderByDescending(t => t.NgayDang)
                .ToList();

            return View(ds);
        }

        [Authorize(Roles = "ChuTro,QuanLy")]
        public IActionResult Tao()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ChuTro,QuanLy")]
        public async Task<IActionResult> Tao(ThongBaoViewModel vm, int SoNgayHienThi)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                vm.NguoiDang = user.UserName;
                vm.NgayDang = DateTime.Now;
                vm.ThoiGianHetHan = DateTime.Now.AddDays(SoNgayHienThi);

                ThongBaoData.Add(vm);

                TempData["Success"] = "Đăng thông báo thành công.";
                return RedirectToAction("DanhSach");
            }

            return View(vm);
        }

    }

}
