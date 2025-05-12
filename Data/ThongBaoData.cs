using QuanLiPhongTro.Models;
using System.Text.Json;
namespace QuanLiPhongTro.Data
{

    public static class ThongBaoData
    {
        private static readonly string _filePath = "Data/thongbao.json";
        public static List<ThongBaoViewModel> DanhSachThongBao { get; private set; } = new();

        static ThongBaoData()
        {
            LoadFromFile();
        }

        public static void Add(ThongBaoViewModel tb)
        {
            DanhSachThongBao.Add(tb);
            SaveToFile();
        }

        public static void CleanExpired()
        {
            DanhSachThongBao = DanhSachThongBao
                .Where(tb => tb.ThoiGianHetHan > DateTime.Now)
                .ToList();

            SaveToFile();
        }

        private static void LoadFromFile()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                DanhSachThongBao = JsonSerializer.Deserialize<List<ThongBaoViewModel>>(json)
                                    ?? new List<ThongBaoViewModel>();
            }

            CleanExpired();
        }

        private static void SaveToFile()
        {
            var json = JsonSerializer.Serialize(DanhSachThongBao);
            File.WriteAllText(_filePath, json);
        }
    }
}
