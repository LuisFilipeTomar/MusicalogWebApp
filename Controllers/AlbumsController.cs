using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using WebApplication1.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Controllers
{
    public class AlbumsController : Controller
    {
        private readonly MusicalogContext _context;

        public AlbumsController(MusicalogContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var musicalogContext = _context.Albums.Include(a => a.Album_Type);
            return View(await musicalogContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string uri = "https://musicalogwebapi.azurewebsites.net/api/Albums/" + id;

            var album = new Models.Album();

            using (HttpClient httpClient = new HttpClient())
            {
                album = JsonConvert.DeserializeObject<Models.Album>(await new HttpClient().GetStringAsync(uri));
            }

            if (album == null)
            {
                return NotFound();
            }

            ViewData["Album_TypeID"] = new SelectList(_context.AlbumTypes, "Album_TypeID", "Album_TypeDesc", album.Album_TypeID);

            return View(album);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            ViewData["Album_TypeID"] = new SelectList(_context.AlbumTypes, "Album_TypeID", "Album_TypeDesc");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Album_ID,Album_Title,Album_ArtistName,Album_TypeID,Album_Stock,Album_Cover")] Album album)
        {
            if (ModelState.IsValid)
            {
                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Album_TypeID"] = new SelectList(_context.AlbumTypes, "Album_TypeID", "Album_TypeDesc", album.Album_TypeID);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string uri = "https://musicalogwebapi.azurewebsites.net/api/Albums/" + id;

            var album = new Models.Album();

            using (HttpClient httpClient = new HttpClient())
            {
                album = JsonConvert.DeserializeObject<Models.Album>(await new HttpClient().GetStringAsync(uri));
            }

            if (album == null)
            {
                return NotFound();
            }
            
            ViewData["Album_TypeID"] = new SelectList(_context.AlbumTypes, "Album_TypeID", "Album_TypeDesc", album.Album_TypeID);

            return View(album);

            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var album = await _context.Albums.FindAsync(id);
            //if (album == null)
            //{
            //    return NotFound();
            //}
            //ViewData["Album_TypeID"] = new SelectList(_context.AlbumTypes, "Album_TypeID", "Album_TypeDesc", album.Album_TypeID);
            //return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile Album_Cover_File, [Bind("Album_ID,Album_Title,Album_ArtistName,Album_TypeID,Album_Stock,Album_Cover")] Album album)
        {
            if (id != album.Album_ID)
            {
                return NotFound();
            }

            if(Album_Cover_File != null && Album_Cover_File.Length > 0)
            {
                MemoryStream vStream = new();
                Album_Cover_File.CopyTo(vStream);
                album.Album_Cover = vStream.ToArray();
            }
            else
            {
                album.Album_Cover = ViewData["Album_Cover"] as byte[];
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.Album_ID))
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
            ViewData["Album_TypeID"] = new SelectList(_context.AlbumTypes, "Album_TypeID", "Album_TypeDesc", album.Album_TypeID);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Album_Type)
                .FirstOrDefaultAsync(m => m.Album_ID == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Album_ID == id);
        }
    }
}
