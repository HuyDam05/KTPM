using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiTrongTrot.Model
{
    // Bảng PhanBon - Phân bón
    public class PhanBon
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string ThanhPhan { get; set; }
        public string PhanLoai { get; set; }
        public int VTId { get; set; }
    }

    // Bảng CoSoSanXuatPhanBon - Cơ sở sản xuất phân bón
    public class CoSoSanXuatPhanBon
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public int BanDoId { get; set; }
        public int LoaiId { get; set; }
    }

    // Bảng CoSoBanPhanBon - Cơ sở bán phân bón
    public class CoSoBanPhanBon
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public int BanDoId { get; set; }
        public int LoaiId { get; set; }
    }

    // Bảng trung gian PhanBon_CoSoSanXuat
    public class PhanBon_CoSoSanXuat
    {
        public int PhanBonId { get; set; }
        public int CoSoSanXuatId { get; set; }
    }

    // Bảng trung gian PhanBon_CoSoBan
    public class PhanBon_CoSoBan
    {
        public int PhanBonId { get; set; }
        public int CoSoBanId { get; set; }
    }
}
