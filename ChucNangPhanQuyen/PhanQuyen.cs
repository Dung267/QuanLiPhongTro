using System.ComponentModel.DataAnnotations;

namespace QuanLiPhongTro.ChucNangPhanQuyen
{
    public enum PhanQuyen //enum
    {
        [Display(Name = "Người thuê")]
        User=1,
        [Display(Name = "Quản lý")]
        QuanLi=2,
        [Display(Name = "Chủ trọ")]
        ChuTro=3


    }
}
