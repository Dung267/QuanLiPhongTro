using Microsoft.AspNetCore.Mvc;
using QuanLiPhongTro.Models;
using System.Diagnostics;

namespace QuanLiPhongTro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("ChuTro"))
                    return RedirectToAction("DashboardChuTro");

                if (User.IsInRole("QuanLi"))
                    return RedirectToAction("DashboardQuanLi");

                if (User.IsInRole("User"))
                    return RedirectToAction("DashboardNguoiThue");
            }

            // Trang public n?u ch?a ??ng nh?p
            return View();
        }
        public IActionResult DashboardChuTro()
        {
            return View();
        }

        public IActionResult DashboardQuanLi()
        {
            return View();
        }

        public IActionResult DashboardNguoiThue()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
