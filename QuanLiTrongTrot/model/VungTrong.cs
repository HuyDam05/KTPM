using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiTrongTrot.Model
{
    // Bảng VungTrong - Vùng trồng
    public class VungTrong
    {
        public int Id { get; set; }
        public double QuyMo { get; set; }      // FLOAT trong SQL -> double trong C#
        public string DiaChi { get; set; }     // NVARCHAR(50)
        public int BanDoId { get; set; }       // Foreign key đến BanDoPhanBo
    }
}
