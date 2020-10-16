using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.Models
{
    public class LoaiSanPham
    {
        public int ID { get; set; }
        public string Ten { get; set; }

        public ICollection<SanPham> LstSanPham { get; set; }
    }
}
