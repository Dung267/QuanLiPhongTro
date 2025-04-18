﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLiPhongTro.Models
{
    [Table("SuDungDichVu")]
    public class SuDungDichVu
    {
        [Key]
        public int Id { get; set; }
        public int ChiSoCu { get; set; }
        public int ChiSoMoi { get; set; }
        public DateTime ThangNam { get; set; }

        public int DichVuId { get; set; }
        public DichVu DichVu { get; set; }

        public int PhongId { get; set; }
        public Phong Phong { get; set; }
    }
}
