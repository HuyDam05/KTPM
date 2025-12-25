using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiTrongTrot.Model
{
    // Giống cây trồng chính
    public class GiongCayChinh
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string PhanLoai { get; set; }
        public string MuaVu { get; set; }
        public int SanLuong { get; set; }
    }

    // Giống cây lưu hành
    public class GiongCayLuuHanh
    {
        public int Id { get; set; }
        public string LoaiCay { get; set; }
        public string NoiPhoBien { get; set; }
        public string CongDung { get; set; }
        public string DacDiem { get; set; }
    }
    // Giống cây đầu dòng
    public class GiongCayDauDong
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string NguonGoc { get; set; }
        public string DacTinh { get; set; }
        public int ThoiGianThuHoach { get; set; }
        public int GiongId { get; set; }
        public int VTId { get; set; }
    }
}
