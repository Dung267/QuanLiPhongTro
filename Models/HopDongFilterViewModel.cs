namespace QuanLiPhongTro.Models
{
    public class HopDongFilterViewModel
    {
        public string? ToaNha { get; set; }
        public string? Phong { get; set; }
        public bool? TrangThai { get; set; }
        public DateTime? Thang { get; set; }
        public string? TuKhoa { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TotalRecords { get; set; }

        public List<HopDong> HopDongs { get; set; } = new();
    }
}
