using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiTrongTrot.Model
{
    // Bảng SinhVatGayHai - Sinh vật gây hại
    public class SinhVatGayHai
    {
        public int Id { get; set; }
        public string Ten { get; set; }           // NVARCHAR(50)
        public string PhanLoai { get; set; }      // NVARCHAR(50)
        public string CayAH { get; set; }         // NVARCHAR(50) - Cây ảnh hưởng
        public int VTId { get; set; }             // Foreign key đến VungTrong
    }

    // Bảng CapNhat_SVGH - Cập nhật tình hình sinh vật gây hại
    public class CapNhat_SVGH
    {
        public int Id { get; set; }
        public DateTime NgayGioCN { get; set; }   // DATETIME
        public string TienDo { get; set; }        // NVARCHAR(50)
        public int SVId { get; set; }             // Foreign key đến SinhVatGayHai
    }

    // Bảng TuoiSau - Tuổi sâu và cấp độ phổ biến
    public class TuoiSau
    {
        public int Id { get; set; }
        public string Ten { get; set; }           // NVARCHAR(50)
        public int TuoiSauValue { get; set; }     // INT - Tuổi sâu
        public string DoPhoBien { get; set; }     // NVARCHAR(50)
        public int SVId { get; set; }             // Foreign key đến SinhVatGayHai
    }
}
