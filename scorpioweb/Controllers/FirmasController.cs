using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using scorpioweb.Models;

namespace scorpioweb.Controllers
{
    public class FirmasController : Controller
    {
        private readonly penas2Context _context;

        public FirmasController(penas2Context context)
        {
            _context = context;
        }

        // GET: Firmas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Firmas.ToListAsync());
        }

        // GET: Firmas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmas = await _context.Firmas
                .SingleOrDefaultAsync(m => m.Idfirmas == id);
            if (firmas == null)
            {
                return NotFound();
            }

            return View(firmas);
        }

        // GET: Firmas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Firmas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idfirmas,Nombre,Fecha,Libro,Foja")] Firmas firmas)
        {
            if (ModelState.IsValid)
            {
                _context.Add(firmas);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(firmas);
        }

        // GET: Firmas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmas = await _context.Firmas.SingleOrDefaultAsync(m => m.Idfirmas == id);
            if (firmas == null)
            {
                return NotFound();
            }
            return View(firmas);
        }

        // POST: Firmas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Idfirmas,Nombre,Fecha,Libro,Foja")] Firmas firmas)
        {
            if (id != firmas.Idfirmas)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(firmas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FirmasExists(firmas.Idfirmas))
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
            return View(firmas);
        }

        // GET: Firmas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var firmas = await _context.Firmas
                .SingleOrDefaultAsync(m => m.Idfirmas == id);
            if (firmas == null)
            {
                return NotFound();
            }

            return View(firmas);
        }

        // POST: Firmas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var firmas = await _context.Firmas.SingleOrDefaultAsync(m => m.Idfirmas == id);
            _context.Firmas.Remove(firmas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FirmasExists(int id)
        {
            return _context.Firmas.Any(e => e.Idfirmas == id);
        }
    }
}
