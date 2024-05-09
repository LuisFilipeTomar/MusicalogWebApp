using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AlbumTypesController : Controller
    {
        private readonly MusicalogContext _context;

        public AlbumTypesController(MusicalogContext context)
        {
            _context = context;
        }

        // GET: AlbumTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AlbumTypes.ToListAsync());
        }

        // GET: AlbumTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumType = await _context.AlbumTypes
                .FirstOrDefaultAsync(m => m.Album_TypeID == id);
            if (albumType == null)
            {
                return NotFound();
            }

            return View(albumType);
        }

        // GET: AlbumTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AlbumTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Album_TypeID,Album_TypeDesc")] AlbumType albumType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(albumType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(albumType);
        }

        // GET: AlbumTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumType = await _context.AlbumTypes.FindAsync(id);
            if (albumType == null)
            {
                return NotFound();
            }
            return View(albumType);
        }

        // POST: AlbumTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Album_TypeID,Album_TypeDesc")] AlbumType albumType)
        {
            if (id != albumType.Album_TypeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(albumType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumTypeExists(albumType.Album_TypeID))
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
            return View(albumType);
        }

        // GET: AlbumTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var albumType = await _context.AlbumTypes
                .FirstOrDefaultAsync(m => m.Album_TypeID == id);
            if (albumType == null)
            {
                return NotFound();
            }

            return View(albumType);
        }

        // POST: AlbumTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albumType = await _context.AlbumTypes.FindAsync(id);
            if (albumType != null)
            {
                _context.AlbumTypes.Remove(albumType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumTypeExists(int id)
        {
            return _context.AlbumTypes.Any(e => e.Album_TypeID == id);
        }
    }
}
