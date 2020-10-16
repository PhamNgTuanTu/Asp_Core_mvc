using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SP.Models;

namespace SP.Data
{
    public class SPContext : DbContext
    {
        public SPContext (DbContextOptions<SPContext> options)
            : base(options)
        {
        }

        public DbSet<SP.Models.SanPham> SanPham { get; set; }

        public DbSet<SP.Models.LoaiSanPham> LoaiSanPham { get; set; }
    }
}
