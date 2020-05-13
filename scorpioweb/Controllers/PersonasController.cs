using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using scorpioweb.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Globalization;

namespace scorpioweb.Controllers
{
    public class PersonasController : Controller
    {

        #region -Variables Globales-
        private readonly penas2Context _context;
        public static int contadorSustancia = 0;
        public static List<List<string>> datosSustancias =new List<List<string>>();
        public static List<List<string>> datosFamiliares = new List<List<string>>();
        public static List<List<string>> datosReferencias = new List<List<string>>();
        public static List<List<string>> datosFamiliaresExtranjero = new List<List<string>>();
        #endregion
        
        public PersonasController(penas2Context context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {            
            return View(await _context.Persona.ToListAsync());
        }

        #region -Detalles-

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .SingleOrDefaultAsync(m => m.IdPersona == id);

            List<Persona> personaVM = _context.Persona.ToList();
            List<Domicilio> domicilioVM = _context.Domicilio.ToList();
            List<Estudios> estudiosVM = _context.Estudios.ToList();
            List<Estados> estados = _context.Estados.ToList();
            List<Municipios> municipios = _context.Municipios.ToList();
            List<Domiciliosecundario> domicilioSecundarioVM = _context.Domiciliosecundario.ToList();
            List<Consumosustancias> consumoSustanciasVM = _context.Consumosustancias.ToList();

            ViewData["joinTables"] = from personaTable in personaVM
                                     join domicilio in domicilioVM on persona.IdPersona equals domicilio.PersonaIdPersona
                                     join estudios in estudiosVM on persona.IdPersona equals estudios.PersonaIdPersona
                                     join nacimientoEstado in estados on (Int32.Parse(persona.Lnestado)) equals nacimientoEstado.Id
                                     join nacimientoMunicipio in municipios on (Int32.Parse(persona.Lnmunicipio)) equals nacimientoMunicipio.Id
                                     join domicilioEstado in estados on (Int32.Parse(domicilio.Estado)) equals domicilioEstado.Id
                                     join domicilioMunicipio in municipios on (Int32.Parse(domicilio.Municipio)) equals domicilioMunicipio.Id
                                     where personaTable.IdPersona == id
                                    select new PersonaViewModel
                                    {
                                        personaVM = personaTable,
                                        domicilioVM = domicilio,
                                        estudiosVM = estudios,
                                        estadosVMPersona=nacimientoEstado,
                                        municipiosVMPersona=nacimientoMunicipio,
                                        estadosVMDomicilio = domicilioEstado,
                                        municipiosVMDomicilio= domicilioMunicipio,
                                    };

            ViewData["joinTablesDomSec"] = from personaTable in personaVM
                                     join domicilio in domicilioVM on persona.IdPersona equals domicilio.PersonaIdPersona
                                           join domicilioSec in domicilioSecundarioVM.DefaultIfEmpty() on domicilio.IdDomicilio equals domicilioSec.IdDomicilio
                                           join domicilioSecEstado in estados on (Int32.Parse(domicilioSec.Estado)) equals domicilioSecEstado.Id
                                           join domicilioSecMunicipio in municipios on (Int32.Parse(domicilioSec.Municipio)) equals domicilioSecMunicipio.Id
                                           where personaTable.IdPersona == id
                                     select new PersonaViewModel
                                     {
                                         domicilioSecundarioVM = domicilioSec,
                                         estadosVMDomicilioSec = domicilioSecEstado,
                                         municipiosVMDomicilioSec = domicilioSecMunicipio
                                     };

            ViewData["joinTablesConsumoSustancias"] = from personaTable in personaVM
                                           join sustancias in consumoSustanciasVM on persona.IdPersona equals sustancias.PersonaIdPersona
                                           where personaTable.IdPersona == id
                                           select new PersonaViewModel
                                           {
                                               consumoSustanciasVM = sustancias
                                           };

            if (persona == null)
            {
                return NotFound();
            }

            return View();
        }
        #endregion


        #region -Entrevista de encuadre insertar-

        public ActionResult guardarSustancia(string[] datosConsumo)
        {
            for (int i = 0; i < datosConsumo.Length - 1; i++)
            {
                datosSustancias.Add(new List<String> { datosConsumo[i], datosConsumo[5] });
            }

            return Json(new { success = true, responseText = "Datos Guardados con éxito" });

        }

        public ActionResult guardarFamiliar(string[] datosFamiliar, int tipoGuardado)
        {
            if (tipoGuardado == 1)
            {
                for (int i = 0; i < datosFamiliar.Length - 1; i++)
                {
                    datosFamiliares.Add(new List<String> { datosFamiliar[i], datosFamiliar[13] });
                }
            }
            else if (tipoGuardado == 2)
            {
                for (int i = 0; i < datosFamiliar.Length - 1; i++)
                {
                    datosReferencias.Add(new List<String> { datosFamiliar[i], datosFamiliar[13] });
                }
            }


            return Json(new { success = true, responseText = "Datos Guardados con éxito" });

        }

        public ActionResult guardarFamiliarExtranjero(string[] datosFE)
        {
            for (int i = 0; i < datosFE.Length - 1; i++)
            {
                datosFamiliaresExtranjero.Add(new List<String> { datosFE[i], datosFE[12] });
            }

            return Json(new { success = true, responseText = "Datos Guardados con éxito" });

        }

        public JsonResult GetMunicipio(int EstadoId)
        {
            TempData["message"] = DateTime.Now;


            List<Municipios> municipiosList = new List<Municipios>();

            municipiosList = (from Municipios in _context.Municipios
                              where Municipios.EstadosId == EstadoId
                              select Municipios).ToList();

            return Json(new SelectList(municipiosList, "Id", "Municipio"));
        }

        // GET: Personas/Create
        public IActionResult Create(Estados Estados)
        {
            //datosSustancias.Clear();
            List<Estados> listaEstados = new List<Estados>();
            listaEstados = (from table in _context.Estados
                            select table).ToList();

            listaEstados.Insert(0, new Estados { Id = 0, Estado = "Selecciona" });
            ViewBag.ListadoEstados = listaEstados;
            return View();
        }

        public string normaliza(string normalizar)
        {
            if (!String.IsNullOrEmpty(normalizar))
            {
                normalizar = normalizar.ToUpper();
            }
            return normalizar;
        }

        public static DateTime validateDatetime(string value)
        {
            try
            {
                return DateTime.Parse(value, new System.Globalization.CultureInfo("pt-BR"));
            }
            catch
            {
                return DateTime.ParseExact("1900/01/01", "yyyy/MM/dd", CultureInfo.InvariantCulture);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Persona persona, Domicilio domicilio, Estudios estudios, Trabajo trabajo, Actividadsocial actividadsocial, Abandonoestado abandonoEstado, Saludfisica saludfisica, Domiciliosecundario domiciliosecundario, Consumosustancias consumosustanciasBD, Asientofamiliar asientoFamiliar, Familiaresforaneos familiaresForaneos,
            string nombre, string paterno, string materno, string alias, string sexo, int edad, DateTime fNacimiento, string lnPais,
            string lnEstado, string lnMunicipio, string lnLocalidad, string estadoCivil, string duracion, string otroIdioma, string especifiqueIdioma, 
            string leerEscribir, string traductor, string especifiqueTraductor, string telefonoFijo, string celular, string hijos, int nHijos, int nPersonasVive,
            string propiedades, string CURP, string consumoSustancias,
            string tipoDomicilio, string calle, string no, string nombreCF, string paisD, string estadoD, string municipioD, string temporalidad,
            string residenciaHabitual, string cp, string referencias, string horario, string observaciones, string cuentaDomicilioSecundario,
            string motivoDS,string tipoDomicilioDS, string calleDS, string noDS, string nombreCFDS, string paisDDS, string estadoDDS, string municipioDDS, string temporalidadDS,
            string residenciaHabitualDS, string cpDS, string referenciasDS, string horarioDS, string observacionesDS,
            string estudia, string gradoEstudios, string institucionE, string horarioE, string direccionE, string telefonoE, string observacionesE,
            string trabaja, string tipoOcupacion, string puesto, string empleadorJefe, string enteradoProceso, string sePuedeEnterar, string tiempoTrabajando,
            string salario, string temporalidadSalario, string direccionT, string horarioT, string telefonoT, string observacionesT,
            string tipoActividad, string horarioAS, string lugarAS, string telefonoAS, string sePuedeEnterarAS, string referenciaAS, string observacionesAS,
            string vividoFuera, string lugaresVivido, string tiempoVivido, string motivoVivido, string viajaHabitual, string lugaresViaje, string tiempoViaje,
            string motivoViaje, string documentaciónSalirPais, string pasaporte, string visa, string familiaresFuera,
            string enfermedad, string especifiqueEnfermedad, string embarazoLactancia, string tiempoEmbarazo, string tratamiento, string discapacidad, string especifiqueDiscapacidad,
            string servicioMedico, string especifiqueServicioMedico, string institucionServicioMedico, string observacionesSalud)//[Bind("IdPersona,Nombre,Paterno,Materno,Alias,Genero,Edad,Fnacimiento,Lnpais,Lnestado,Lnmunicipio,Lnlocalidad,EstadoCivil,Duracion,OtroIdioma,EspecifiqueIdioma,DatosGeneralescol,LeerEscribir,Traductor,EspecifiqueTraductor,TelefonoFijo,Celular,Hijos,Nhijos,NpersonasVive,Propiedades,Curp,ConsumoSustancias,UltimaActualización")]
        {


            if (ModelState.ErrorCount <= 1)
            {
                #region -Persona-            
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
                persona.EstadoCivil = estadoCivil;
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
                persona.UltimaActualización = DateTime.Now;
                #endregion

                #region -Domicilio-
                domicilio.TipoDomicilio = tipoDomicilio;
                domicilio.Calle = normaliza(calle);
                domicilio.No = normaliza(no);
                domicilio.NombreCf = normaliza(nombreCF);
                domicilio.Pais = paisD;
                domicilio.Estado = estadoD;
                domicilio.Municipio = municipioD;
                domicilio.Temporalidad = temporalidad;
                domicilio.ResidenciaHabitual = normaliza(residenciaHabitual);
                domicilio.Cp = normaliza(cp);
                domicilio.Referencias = normaliza(referencias);
                domicilio.DomcilioSecundario = cuentaDomicilioSecundario;
                domicilio.Horario = normaliza(horario);
                domicilio.Observaciones = normaliza(observaciones);
                #endregion

                #region -Domicilio Secundario-
                domiciliosecundario.Motivo = motivoDS;
                domiciliosecundario.TipoDomicilio = tipoDomicilioDS;
                domiciliosecundario.Calle = normaliza(calleDS);
                domiciliosecundario.No = normaliza(noDS);
                domiciliosecundario.NombreCf = normaliza(nombreCFDS);
                domiciliosecundario.Pais = paisDDS;
                domiciliosecundario.Estado = estadoDDS;
                domiciliosecundario.Municipio = municipioDDS;
                domiciliosecundario.Temporalidad = temporalidadDS;                
                domiciliosecundario.ResidenciaHabitual = normaliza(residenciaHabitualDS);
                domiciliosecundario.Cp = normaliza(cpDS);
                domiciliosecundario.Referencias = normaliza(referenciasDS);
                domiciliosecundario.Horario = normaliza(horarioDS);
                domiciliosecundario.Observaciones = normaliza(observacionesDS);
                #endregion

                #region -Estudios-
                estudios.Estudia = estudia;
                estudios.GradoEstudios = gradoEstudios;
                estudios.InstitucionE = normaliza(institucionE);
                estudios.Horario = normaliza(horarioE);
                estudios.Direccion = normaliza(direccionE);
                estudios.Telefono = normaliza(telefonoE);
                estudios.Observaciones = normaliza(observacionesE);
                #endregion

                #region -Trabajo-
                trabajo.Trabaja = trabaja;
                trabajo.TipoOcupacion = tipoOcupacion;
                trabajo.Puesto = normaliza(puesto);
                trabajo.EmpledorJefe = normaliza(empleadorJefe);
                trabajo.EnteradoProceso = enteradoProceso;
                trabajo.SePuedeEnterar = sePuedeEnterar;
                trabajo.TiempoTrabajano = tiempoTrabajando;
                trabajo.Salario = normaliza(salario);
                trabajo.Direccion = normaliza(direccionT);
                trabajo.Horario = normaliza(horarioT);
                trabajo.Telefono = normaliza(telefonoT);
                trabajo.Observaciones = normaliza(observacionesT);
                #endregion

                #region -ActividadSocial-
                actividadsocial.TipoActividad = normaliza(tipoActividad);
                actividadsocial.Horario = normaliza(horarioAS);
                actividadsocial.Lugar = normaliza(lugarAS);
                actividadsocial.Telefono = normaliza(telefonoAS);
                actividadsocial.SePuedeEnterar = sePuedeEnterarAS;
                actividadsocial.Referencia = normaliza(referenciaAS);
                actividadsocial.Observaciones = normaliza(observacionesAS);
                #endregion

                #region -AbandonoEstado-
                abandonoEstado.VividoFuera = vividoFuera;
                abandonoEstado.LugaresVivido = normaliza(lugaresVivido);
                abandonoEstado.TiempoVivido = normaliza(tiempoVivido);
                abandonoEstado.MotivoVivido = normaliza(motivoVivido);
                abandonoEstado.ViajaHabitual = viajaHabitual;
                abandonoEstado.LugaresViaje = normaliza(lugaresViaje);
                abandonoEstado.TiempoViaje = normaliza(tiempoViaje);
                abandonoEstado.MotivoViaje = normaliza(motivoViaje);
                abandonoEstado.DocumentacionSalirPais = documentaciónSalirPais;
                abandonoEstado.Pasaporte = pasaporte;
                abandonoEstado.Visa = visa;
                abandonoEstado.FamiliaresFuera = familiaresFuera;
                //abandonoEstado.Cuantos = cuantosFamiliares;
                #endregion

                #region -Salud-
                saludfisica.Enfermedad = enfermedad;
                saludfisica.EspecifiqueEnfermedad = normaliza(especifiqueEnfermedad);
                saludfisica.EmbarazoLactancia = embarazoLactancia;
                saludfisica.Tiempo = normaliza(tiempoEmbarazo);
                saludfisica.Tratamiento = normaliza(tratamiento);
                saludfisica.Discapacidad = discapacidad;
                saludfisica.EspecifiqueDiscapacidad = normaliza(especifiqueDiscapacidad);
                saludfisica.ServicioMedico = servicioMedico;
                saludfisica.EspecifiqueServicioMedico = especifiqueServicioMedico;
                saludfisica.InstitucionServicioMedico = institucionServicioMedico;
                saludfisica.Observaciones = normaliza(observacionesSalud);
                #endregion

                #region -IdDomicilio-
                int idDomicilio = ((from table in _context.Domicilio
                                    select table).Count()) + 1;
                domicilio.IdDomicilio = idDomicilio;
                domiciliosecundario.IdDomicilio = idDomicilio;
                #endregion

                #region -IdPersona-
                int idPersona = ((from table in _context.Persona
                                  select table).Count()) + 1;
                persona.IdPersona = idPersona;
                domicilio.PersonaIdPersona = idPersona;
                estudios.PersonaIdPersona = idPersona;
                trabajo.PersonaIdPersona = idPersona;
                actividadsocial.PersonaIdPersona = idPersona;
                abandonoEstado.PersonaIdPersona = idPersona;
                saludfisica.PersonaIdPersona = idPersona;
                #endregion

                #region -ConsumoSustancias-
                for (int i=0; i< datosSustancias.Count;i=i+5)
                {
                    if (datosSustancias[i][1] == "iovanni")
                    { /*Revisar el cambio de variable "iovanni" por a variable de usuario*/ 
                        consumosustanciasBD.Sustancia = datosSustancias[i][0];
                        consumosustanciasBD.Frecuencia = datosSustancias[i + 1][0];
                        consumosustanciasBD.Cantidad = normaliza(datosSustancias[i + 2][0]);
                        consumosustanciasBD.UltimoConsumo = validateDatetime(datosSustancias[i + 3][0]);
                        consumosustanciasBD.Observaciones = normaliza(datosSustancias[i + 4][0]);
                        consumosustanciasBD.PersonaIdPersona = idPersona;
                        _context.Add(consumosustanciasBD);
                        await _context.SaveChangesAsync();
                    }
                }
                #endregion

                #region -AsientoFamiliar-
                for(int i=0; i < datosFamiliares.Count; i = i + 13)
                {
                    if (datosFamiliares[i][1] == "iovanni")
                    {
                        asientoFamiliar.Nombre = normaliza(datosFamiliares[i][0]);
                        asientoFamiliar.Relacion = datosFamiliares[i + 1][0];
                        asientoFamiliar.Edad = Int32.Parse(datosFamiliares[i+2][0]);
                        asientoFamiliar.Sexo = datosFamiliares[i + 3][0];
                        asientoFamiliar.Dependencia = datosFamiliares[i+4][0];
                        asientoFamiliar.DependenciaExplica = normaliza(datosFamiliares[i+5][0]);
                        asientoFamiliar.VivenJuntos = datosFamiliares[i+6][0];
                        asientoFamiliar.Domicilio = normaliza(datosFamiliares[i + 7][0]);
                        asientoFamiliar.Telefono = datosFamiliares[i + 8][0];
                        asientoFamiliar.HorarioLocalizacion = normaliza(datosFamiliares[i + 9][0]);
                        asientoFamiliar.EnteradoProceso = datosFamiliares[i + 10][0];
                        asientoFamiliar.PuedeEnterarse = datosFamiliares[i + 11][0];
                        asientoFamiliar.Observaciones = normaliza(datosFamiliares[i + 12][0]);
                        asientoFamiliar.Tipo = "FAMILIAR";
                        asientoFamiliar.PersonaIdPersona = idPersona;
                        _context.Add(asientoFamiliar);
                        await _context.SaveChangesAsync();
                    }
                }
                #endregion

                #region -Referencias-
                for (int i = 0; i < datosReferencias.Count; i = i + 13)
                {
                    if (datosReferencias[i][1] == "iovanni")
                    {
                        asientoFamiliar.Nombre = normaliza(datosReferencias[i][0]);
                        asientoFamiliar.Relacion = datosReferencias[i + 1][0];
                        asientoFamiliar.Edad = Int32.Parse(datosReferencias[i + 2][0]);
                        asientoFamiliar.Sexo = datosReferencias[i + 3][0];
                        asientoFamiliar.Dependencia = datosReferencias[i + 4][0];
                        asientoFamiliar.DependenciaExplica = normaliza(datosReferencias[i + 5][0]);
                        asientoFamiliar.VivenJuntos = datosReferencias[i + 6][0];
                        asientoFamiliar.Domicilio = normaliza(datosReferencias[i + 7][0]);
                        asientoFamiliar.Telefono = datosReferencias[i + 8][0];
                        asientoFamiliar.HorarioLocalizacion = normaliza(datosReferencias[i + 9][0]);
                        asientoFamiliar.EnteradoProceso = datosReferencias[i + 10][0];
                        asientoFamiliar.PuedeEnterarse = datosReferencias[i + 11][0];
                        asientoFamiliar.Observaciones = normaliza(datosReferencias[i + 12][0]);
                        asientoFamiliar.Tipo = "REFERENCIA";
                        asientoFamiliar.PersonaIdPersona = idPersona;
                        _context.Add(asientoFamiliar);
                        await _context.SaveChangesAsync();
                    }
                }
                #endregion

                #region -Familiares Extranjero-
                for (int i = 0; i < datosFamiliaresExtranjero.Count; i = i + 12)
                {
                    if (datosFamiliaresExtranjero[i][1] == "iovanni")
                    {
                        familiaresForaneos.Nombre = normaliza(datosFamiliaresExtranjero[i][0]);
                        familiaresForaneos.Relacion = datosFamiliaresExtranjero[i + 1][0];
                        familiaresForaneos.Edad = Int32.Parse(datosFamiliaresExtranjero[i + 2][0]);
                        familiaresForaneos.Sexo = datosFamiliaresExtranjero[i + 3][0];
                        familiaresForaneos.TiempoConocerlo = datosFamiliaresExtranjero[i + 4][0];
                        familiaresForaneos.Pais = datosFamiliaresExtranjero[i + 5][0];
                        familiaresForaneos.Estado = normaliza(datosFamiliaresExtranjero[i + 6][0]);
                        familiaresForaneos.Telefono = normaliza(datosFamiliaresExtranjero[i + 7][0]);
                        familiaresForaneos.FrecuenciaContacto = datosFamiliaresExtranjero[i + 8][0];
                        familiaresForaneos.EnteradoProceso = datosFamiliaresExtranjero[i + 9][0];
                        familiaresForaneos.PuedeEnterarse = datosFamiliaresExtranjero[i + 10][0];
                        familiaresForaneos.Observaciones = normaliza(datosFamiliaresExtranjero[i + 11][0]);
                        familiaresForaneos.PersonaIdPersona = idPersona;
                        _context.Add(familiaresForaneos);
                        await _context.SaveChangesAsync();
                    }
                }
                #endregion

                #region -Añadir a contexto-
                _context.Add(persona);
                _context.Add(domicilio);
                _context.Add(domiciliosecundario);
                _context.Add(estudios);
                _context.Add(trabajo);
                _context.Add(actividadsocial);
                _context.Add(abandonoEstado);
                _context.Add(saludfisica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                #endregion
            }
            return View(persona);
        }

        #endregion

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
