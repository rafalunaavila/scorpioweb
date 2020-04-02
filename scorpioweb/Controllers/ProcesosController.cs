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
    public class ProcesosController : Controller
    {
        private readonly penas2Context _context;

        public ProcesosController(penas2Context context)
        {
            _context = context;
        }

        // GET: Procesos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Proceso.ToListAsync());
        }

        // GET: Procesos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proceso = await _context.Proceso
                .SingleOrDefaultAsync(m => m.IdProceso == id);
            if (proceso == null)
            {
                return NotFound();
            }

            return View(proceso);
        }

        // GET: Procesos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Procesos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProceso,CausaPenal,Coimputados,CoimputadosSupervision,NoCoimputadosSupervision,RelacionSupervisado,RelacionLugar,EspecificaRelacion,Funcionario,EspecificaFuncionario,Observaciones,PersonaIdPersona")] Proceso proceso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(proceso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proceso);
        }

        // GET: Procesos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proceso = await _context.Proceso.SingleOrDefaultAsync(m => m.IdProceso == id);
            if (proceso == null)
            {
                return NotFound();
            }
            return View(proceso);
        }

        // POST: Procesos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProceso,CausaPenal,Coimputados,CoimputadosSupervision,NoCoimputadosSupervision,RelacionSupervisado,RelacionLugar,EspecificaRelacion,Funcionario,EspecificaFuncionario,Observaciones,PersonaIdPersona")] Proceso proceso)
        {
            if (id != proceso.IdProceso)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proceso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcesoExists(proceso.IdProceso))
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
            return View(proceso);
        }

        // GET: Procesos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proceso = await _context.Proceso
                .SingleOrDefaultAsync(m => m.IdProceso == id);
            if (proceso == null)
            {
                return NotFound();
            }

            return View(proceso);
        }

        // POST: Procesos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proceso = await _context.Proceso.SingleOrDefaultAsync(m => m.IdProceso == id);
            _context.Proceso.Remove(proceso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProcesoExists(int id)
        {
            return _context.Proceso.Any(e => e.IdProceso == id);
        }
    }
}
