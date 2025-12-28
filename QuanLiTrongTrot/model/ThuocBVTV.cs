using System;

namespace QuanLiTrongTrot.Model
{
    // Bảng ThuocBVTV - Thuốc bảo vệ thực vật
    public class ThuocBVTV
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public DateTime NgaySX { get; set; }
        public DateTime HanSD { get; set; }
        public int VTId { get; set; }
    }

    // Bảng CoSoSanXuatThuocBVTV - Cơ sở sản xuất thuốc BVTV
    public class CoSoSanXuatThuocBVTV
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public int BanDoId { get; set; }
        public int LoaiId { get; set; }
    }

    // Bảng CoSoBanThuocBVTV - Cơ sở bán thuốc BVTV
    public class CoSoBanThuocBVTV
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public int BanDoId { get; set; }
        public int LoaiId { get; set; }
    }

    // Bảng ThuocBVTV_CoSoBan (bảng trung gian)
    public class ThuocBVTV_CoSoBan
    {
        public int ThuocBVTVId { get; set; }
        public int CoSoBanId { get; set; }
    }

    // Bảng ThuocBVTV_CoSoSanXuat (bảng trung gian)
    public class ThuocBVTV_CoSoSanXuat
    {
        public int ThuocBVTVId { get; set; }
        public int CoSoSanXuatId { get; set; }
    }
}
