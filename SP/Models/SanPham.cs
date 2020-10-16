using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SP.Models
{
    public class SanPham
    {
        public int ID { get; set; }
        public string Ten { get; set; }

        public string Hinh { get; set; }
        public int Maloai { get; set; }
        [ForeignKey("Maloai")]
        public virtual LoaiSanPham Loai { get; set; }
    }
}
