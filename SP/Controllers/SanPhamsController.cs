using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SP.Data;
using SP.Models;

namespace SP.Controllers
{
    public class SanPhamsController : Controller
    {
        private readonly SPContext _context;

        public SanPhamsController(SPContext context)
        {
            _context = context;
        }

        // GET: SanPhams
        public async Task<IActionResult> Index(string SanPhamGenre, string searchString)
        {
            // Use LINQ to get list of genres.
            var genreQuery = (from m in _context.SanPham
                                            orderby m.Maloai
                                            select m.Maloai);

            var SP = from m in _context.SanPham
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                SP = SP.Where(s => s.Ten.ToLower().Contains(searchString));
            }

            if (!string.IsNullOrEmpty(SanPhamGenre))
            {
                SP = SP.Where(x => x.Maloai == int.Parse(SanPhamGenre));
            }

            var SanPhamGenreVM = new SanPhamGenreViewModel
            {
                Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
                SanPhams = await SP.ToListAsync()
            };

            return View(SanPhamGenreVM);
        }

        // GET: SanPhams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.Loai)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // GET: SanPhams/Create
        public IActionResult Create()
        {
            ViewData["Maloai"] = new SelectList(_context.Set<LoaiSanPham>(), "ID", "ID");
            return View();
        }

        // POST: SanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Ten,Hinh,Maloai")] SanPham sanPham, IFormFile ful)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sanPham);
                await _context.SaveChangesAsync();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", sanPham.ID + "." + ful.FileName.Split(".")
                    [ful.FileName.Split(".").Length - 1]);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ful.CopyToAsync(stream);
                }
                sanPham.Hinh = sanPham.ID + "." + ful.FileName.Split(".")[ful.FileName.Split(".").Length - 1];
                _context.Update(sanPham);
                _ = await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["Maloai"] = new SelectList(_context.LoaiSanPham, "ID", "ID", sanPham.Maloai);
            return View(sanPham);
        }

        // GET: SanPhams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham.FindAsync(id);
            if (sanPham == null)
            {
                return NotFound();
            }
            ViewData["Maloai"] = new SelectList(_context.Set<LoaiSanPham>(), "ID", "ID", sanPham.Maloai);
            return View(sanPham);
        }

        // POST: SanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Ten,Hinh,Maloai")] SanPham sanPham, IFormFile ful)
        {
            if (id != sanPham.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ful != null)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", sanPham.Hinh);
                        System.IO.File.Delete(path);

                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", sanPham.ID + "." + ful.FileName.Split(".")
                        [ful.FileName.Split(".").Length - 1]);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await ful.CopyToAsync(stream);
                        }
                        sanPham.Hinh = sanPham.ID + "." + ful.FileName.Split(".")[ful.FileName.Split(".").Length - 1];
                    }
                    _context.Update(sanPham);
                    _ = await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamExists(sanPham.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewBag["Maloai"] = new SelectList(_context.LoaiSanPham, "ID", "ID", sanPham.Maloai);
            return View(sanPham);
        }

        // GET: SanPhams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sanPham = await _context.SanPham
                .Include(s => s.Loai)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (sanPham == null)
            {
                return NotFound();
            }

            return View(sanPham);
        }

        // POST: SanPhams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sanPham = await _context.SanPham.FindAsync(id);
            _context.SanPham.Remove(sanPham);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamExists(int id)
        {
            return _context.SanPham.Any(e => e.ID == id);
        }
    }
}
