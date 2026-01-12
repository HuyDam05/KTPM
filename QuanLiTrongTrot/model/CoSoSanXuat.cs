using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiTrongTrot.Model
{
    // Bảng CS_VG - Cơ sở đủ ATTP VietGap
    public class CS_VG
    {
        public int Id { get; set; }
        public string Ten { get; set; }
        public string DiaChi { get; set; }
        public int BanDoId { get; set; }
    }
}
