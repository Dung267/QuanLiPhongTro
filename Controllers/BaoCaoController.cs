/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLiPhongTro.Data;
using QuanLiPhongTro.Models;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLiPhongTro.Controllers
{
    public class BaoCaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BaoCaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Báo cáo bất động sản: Danh sách phòng sắp trống
        public async Task<IActionResult> BaoCaoBatDongSan()
        {
            // Lấy danh sách phòng sắp trống (hợp đồng kết thúc trong vòng 1 tháng tới)
            var phongSapTrong = await _context.Phongs
                .Where(p => p.TinhTrang == "Đã cho thuê" && p.NgayHetHopDong <= DateTime.Now.AddMonths(1))
                .ToListAsync();

            var baoCaoBatDongSan = new BaoCaoBatDongSan
            {
                PhongSapTrong = phongSapTrong
            };

            return View(baoCaoBatDongSan);  // Trả về view báo cáo bất động sản
        }

        // Báo cáo tài chính
        public async Task<IActionResult> BaoCaoTaiChinh()
        {
            // Tổng doanh thu từ hợp đồng đã thanh toán
            var tongDoanhThu = await _context.HopDongs
                .Where(hd => hd.DaTra)
                .SumAsync(hd => hd.TienCoc + hd.TienPhong);  // Tính doanh thu từ tiền cọc và tiền phòng

            // Lợi nhuận (giả sử lợi nhuận = doanh thu - tiền cọc)
            var loiNhuan = tongDoanhThu - await _context.HopDongs.SumAsync(hd => hd.TienCoc);

            // Tổng khách nợ tiền (hợp đồng chưa thanh toán)
            var khachNoTien = await _context.HopDongs
                .Where(hd => !hd.DaTra)
                .SumAsync(hd => hd.TienPhong);

            // Tổng tiền cọc
            var tienCoc = await _context.HopDongs
                .SumAsync(hd => hd.TienCoc);

            // Tạo báo cáo tài chính
            var baoCaoTaiChinh = new BaoCaoTaiChinh
            {
                TongDoanhThu = tongDoanhThu,
                LoiNhuan = loiNhuan,
                KhachNoTien = khachNoTien,
                TienCoc = tienCoc
            };

            return View(baoCaoTaiChinh);  // Trả về view báo cáo tài chính
        }
    }
}
*/
/* Cập nhật Model
Cập nhật BaoCaoController để thực hiện các báo cáo tài chính và bất động sản dựa trên dữ liệu từ model HopDong và các thực thể liên quan như Phong, TraHopDong (thanh toán hợp đồng).

Model Báo cáo tài chính (BaoCaoTaiChinh)
csharp
Sao chép
public class BaoCaoTaiChinh
{
    public decimal TongDoanhThu { get; set; }  // Tổng doanh thu từ hợp đồng
    public decimal LoiNhuan { get; set; }  // Lợi nhuận (doanh thu trừ chi phí)
    public decimal KhachNoTien { get; set; }  // Tổng tiền nợ từ hợp đồng chưa thanh toán
    public decimal TienCoc { get; set; }  // Tiền cọc (tổng tiền cọc từ các hợp đồng)
}
Model Báo cáo bất động sản (BaoCaoBatDongSan)
csharp
Sao chép
public class BaoCaoBatDongSan
{
    public List<Phong> PhongSapTrong { get; set; }  // Danh sách phòng sắp trống (hợp đồng sắp hết hạn hoặc đã trả)
}
Model Phong (Phong có thể liên quan đến HopDong)
csharp
Sao chép
public class Phong
{
    [Key]
    public string Id { get; set; }
    public string TenPhong { get; set; }
    public string TinhTrang { get; set; }  // Trạng thái phòng: Đã cho thuê / Trống
    public DateTime NgayHetHopDong { get; set; }  // Ngày kết thúc hợp đồng
}*/