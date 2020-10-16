using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.Models
{
    public class SanPhamGenreViewModel
    {
        public List<SanPham> SanPhams { get; set; }
        public SelectList Genres { get; set; }
        public string SanPhamGenre { get; set; }
        public string SearchString { get; set; }
    }
}
