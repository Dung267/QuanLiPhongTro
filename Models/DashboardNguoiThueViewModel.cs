namespace QuanLiPhongTro.Models
{
    public class DashboardNguoiThueViewModel
    {
        // Hợp đồng hiện tại của người thuê (chưa thanh lý)
        public required HopDong HopDongHienTai { get; set; }

        // Danh sách sự cố gần đây mà người thuê đã báo
        public required List<SuCo> SuCos { get; set; }

        // Danh sách các hóa đơn đã thanh toán hoặc đang chờ
        public required List<ThanhToan> HoaDons { get; set; }
    }
}
