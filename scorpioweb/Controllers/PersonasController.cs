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
    public class PersonasController : Controller
    {
        private readonly penas2Context _context;

        

        public PersonasController(penas2Context context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Persona.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .SingleOrDefaultAsync(m => m.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        public JsonResult GetMunicipio(int EstadoId) {
            List<Municipios> municipiosList = new List<Municipios>();

            municipiosList = (from Municipios in _context.Municipios
                              where Municipios.Id == EstadoId
                              select Municipios).ToList();

            return Json(new SelectList(municipiosList, "Id", "Municipio"));
        }

        // GET: Personas/Create
        public IActionResult Create(Estados Estados)
        {
            List<Estados> listaEstados = new List<Estados>();
            listaEstados = (from table in _context.Estados
                            select table).ToList();
            ViewBag.ListadoEstados = listaEstados;
            return View();
        }

        public string normaliza(string normalizar) {
            if (!String.IsNullOrEmpty(normalizar))
            {
                normalizar=normalizar.ToUpper();
            }
            return normalizar;
        }

        // POST: Personas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Persona persona, Proceso proceso, string nombre, string paterno, string materno, string alias, string sexo, int edad, DateTime fNacimiento, string lnPais,
            string lnEstado, string lnMunicipio, string lnLocalidad, string estadoCivil, string duracion, string otroIdioma, string especifiqueIdioma, 
            string leerEscribir, string traductor, string especifiqueTraductor, string telefonoFijo, string celular, string hijos, int nHijos, int nPersonasVive,
            string propiedades, string CURP, string consumoSustancias)//[Bind("IdPersona,Nombre,Paterno,Materno,Alias,Genero,Edad,Fnacimiento,Lnpais,Lnestado,Lnmunicipio,Lnlocalidad,EstadoCivil,Duracion,OtroIdioma,EspecifiqueIdioma,DatosGeneralescol,LeerEscribir,Traductor,EspecifiqueTraductor,TelefonoFijo,Celular,Hijos,Nhijos,NpersonasVive,Propiedades,Curp,ConsumoSustancias,UltimaActualización")]
        {
            int idPersona = ((from table in _context.Persona
                         select table).Count())+1;

            if (ModelState.ErrorCount<=1)
            {
                persona.IdPersona = idPersona;
                persona.Nombre = nombre.ToUpper();
                persona.Paterno = paterno.ToUpper();
                persona.Materno = normaliza(materno);
                persona.Alias = normaliza(alias);
                persona.Genero = normaliza(sexo);
                persona.Edad = edad;
                persona.Fnacimiento = fNacimiento;
                persona.Lnpais = lnPais;
                persona.Lnestado = normaliza(lnEstado);
                persona.Lnmunicipio = lnMunicipio;
                persona.Lnlocalidad = lnLocalidad;
                persona.EstadoCivil = 
                persona.Duracion = duracion;
                persona.OtroIdioma = normaliza(otroIdioma);
                persona.EspecifiqueIdioma = normaliza(especifiqueIdioma);
                persona.LeerEscribir = normaliza(leerEscribir);
                persona.Traductor = normaliza(traductor);
                persona.EspecifiqueTraductor = normaliza(especifiqueTraductor);
                persona.TelefonoFijo = telefonoFijo;
                persona.Celular = celular;
                persona.Hijos = normaliza(hijos);
                persona.Nhijos = nHijos;
                persona.NpersonasVive = nPersonasVive;
                persona.Propiedades = normaliza(propiedades);
                persona.Curp = normaliza(CURP);
                persona.ConsumoSustancias = normaliza(consumoSustancias);
                persona.UltimaActualización= DateTime.Now;
                //proceso.IdProceso = idProceso;
                _context.Add(persona);
                //_context.Add(proceso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona.SingleOrDefaultAsync(m => m.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersona,Nombre,Paterno,Materno,Alias,Genero,Edad,Fnacimiento,Lnpais,Lnestado,Lnmunicipio,Lnlocalidad,EstadoCivil,Duracion,OtroIdioma,EspecifiqueIdioma,DatosGeneralescol,LeerEscribir,Traductor,EspecifiqueTraductor,TelefonoFijo,Celular,Hijos,Nhijos,NpersonasVive,Propiedades,Curp,ConsumoSustancias,UltimaActualización")] Persona persona)
        {
            if (id != persona.IdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.IdPersona))
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
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .SingleOrDefaultAsync(m => m.IdPersona == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Persona.SingleOrDefaultAsync(m => m.IdPersona == id);
            _context.Persona.Remove(persona);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.IdPersona == id);
        }
    }
}
