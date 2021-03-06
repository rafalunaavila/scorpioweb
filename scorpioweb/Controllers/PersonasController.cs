﻿using System;
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
using Rotativa;
using Rotativa.AspNetCore;
using Syncfusion.HtmlConverter;
using Syncfusion.Pdf;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace scorpioweb.Controllers
{
    [Authorize]
    public class PersonasController : Controller
    {

        //To get content root path of the project
        private readonly IHostingEnvironment _hostingEnvironment;

        #region -Variables Globales-
        private readonly penas2Context _context;
        public static int contadorSustancia = 0;
        public static List<List<string>> datosSustancias = new List<List<string>>();
        public static List<List<string>> datosFamiliares = new List<List<string>>();
        public static List<List<string>> datosReferencias = new List<List<string>>();
        public static List<List<string>> datosFamiliaresExtranjero = new List<List<string>>();
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        #endregion

        #region -Constructor-
        public PersonasController(penas2Context context, IHostingEnvironment hostingEnvironment,
                                  RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        #endregion

        public IActionResult ExportToPDF()
        {
            //Initialize HTML to PDF converter 
            HtmlToPdfConverter htmlConverter = new HtmlToPdfConverter();
            WebKitConverterSettings settings = new WebKitConverterSettings();
            //Set WebKit path
            settings.WebKitPath = Path.Combine(_hostingEnvironment.ContentRootPath, "QtBinariesWindows");
            //Assign WebKit settings to HTML converter
            htmlConverter.ConverterSettings = settings;
            //Convert URL to PDF
            PdfDocument document = htmlConverter.Convert("https://localhost:44359/Firmas/GeneraQR");
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Pdf, "Output.pdf");
        }

        // GET: Personas
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;

            var personas = from p in _context.Persona
                           where p.Supervisor != null
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                personas = personas.Where(p => p.Paterno.Contains(searchString)
                                        || p.Materno.Contains(searchString)
                                        || p.Nombre.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    personas = personas.OrderByDescending(p => p.Paterno);
                    break;
                case "Date":
                    personas = personas.OrderBy(p => p.UltimaActualización);
                    break;
                case "date_desc":
                    personas = personas.OrderByDescending(p => p.UltimaActualización);
                    break;
                default:
                    personas = personas.OrderBy(p => p.Paterno);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Persona>.CreateAsync(personas.AsNoTracking(), pageNumber?? 1, pageSize));
        }

        public async Task<IActionResult> ListadoSupervisor(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }


            ViewData["CurrentFilter"] = searchString;

            var personas = from p in _context.Persona
                           where p.Supervisor == User.Identity.Name
                           select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                personas = personas.Where(p => p.Paterno.Contains(searchString)
                                        || p.Materno.Contains(searchString)
                                        || p.Nombre.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    personas = personas.OrderByDescending(p => p.Paterno);
                    break;
                case "Date":
                    personas = personas.OrderBy(p => p.UltimaActualización);
                    break;
                case "date_desc":
                    personas = personas.OrderByDescending(p => p.UltimaActualización);
                    break;
                default:
                    personas = personas.OrderBy(p => p.Paterno);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Persona>.CreateAsync(personas.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        #region -AsignaSupervision-

        public async Task<IActionResult> AsignacionSupervision()
        {
            List<SelectListItem> ListaUsuarios = new List<SelectListItem>();
            int i = 0;
            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "SupervisorMCSCP"))
                {
                    ListaUsuarios.Add(new SelectListItem
                    {
                        Text = user.ToString(),
                        Value = i.ToString()
                    });
                    i++;
                }
            }
            ViewBag.ListadoUsuarios = ListaUsuarios;
            return View(await _context.Persona.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSupervisor(Persona persona)
        {
            int id = persona.IdPersona;
            string supervisor = persona.Supervisor;

            var personaUpdate = await _context.Persona
                .FirstOrDefaultAsync(p => p.IdPersona == id);

            personaUpdate.Supervisor = supervisor;

            try
            {
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
            return RedirectToAction("MenuMCSCP");
        }

        #endregion

        public async Task<IActionResult> MenuMCSCP()
        {            
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var roles = await userManager.GetRolesAsync(user);

            List<string> rolUsuario = new List<string>();

            for (int i = 0; i < roles.Count; i++)
            {
                rolUsuario.Add(roles[i]);
            }

            ViewBag.RolesUsuario = rolUsuario;
            return View();
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

            #region -To List databases-

            List<Persona> personaVM = _context.Persona.ToList();
            List<Domicilio> domicilioVM = _context.Domicilio.ToList();
            List<Estudios> estudiosVM = _context.Estudios.ToList();
            List<Estados> estados = _context.Estados.ToList();
            List<Municipios> municipios = _context.Municipios.ToList();
            List<Domiciliosecundario> domicilioSecundarioVM = _context.Domiciliosecundario.ToList();
            List<Consumosustancias> consumoSustanciasVM = _context.Consumosustancias.ToList();
            List<Trabajo> trabajoVM = _context.Trabajo.ToList();
            List<Actividadsocial> actividadSocialVM = _context.Actividadsocial.ToList();
            List<Abandonoestado> abandonoEstadoVM = _context.Abandonoestado.ToList();
            List<Saludfisica> saludFisicaVM = _context.Saludfisica.ToList();
            List<Familiaresforaneos> familiaresForaneosVM = _context.Familiaresforaneos.ToList();
            List<Asientofamiliar> asientoFamiliarVM = _context.Asientofamiliar.ToList();

            #endregion

            #region -Jointables-
            ViewData["joinTables"] = from personaTable in personaVM
                                     join domicilio in domicilioVM on persona.IdPersona equals domicilio.PersonaIdPersona
                                     join estudios in estudiosVM on persona.IdPersona equals estudios.PersonaIdPersona
                                     join trabajo in trabajoVM on persona.IdPersona equals trabajo.PersonaIdPersona
                                     join actividaSocial in actividadSocialVM on persona.IdPersona equals actividaSocial.PersonaIdPersona
                                     join abandonoEstado in abandonoEstadoVM on persona.IdPersona equals abandonoEstado.PersonaIdPersona
                                     join saludFisica in saludFisicaVM on persona.IdPersona equals saludFisica.PersonaIdPersona
                                     //join nacimientoEstado in estados on (Int32.Parse(persona.Lnestado)) equals nacimientoEstado.Id
                                     //join nacimientoMunicipio in municipios on (Int32.Parse(persona.Lnmunicipio)) equals nacimientoMunicipio.Id
                                     //join domicilioEstado in estados on (Int32.Parse(domicilio.Estado)) equals domicilioEstado.Id
                                     //join domicilioMunicipio in municipios on (Int32.Parse(domicilio.Municipio)) equals domicilioMunicipio.Id
                                     where personaTable.IdPersona == id
                                     select new PersonaViewModel
                                     {
                                         personaVM = personaTable,
                                         domicilioVM = domicilio,
                                         estudiosVM = estudios,
                                         trabajoVM = trabajo,
                                         actividadSocialVM = actividaSocial,
                                         abandonoEstadoVM = abandonoEstado,
                                         saludFisicaVM = saludFisica
                                         //estadosVMPersona=nacimientoEstado,
                                         //municipiosVMPersona=nacimientoMunicipio,
                                         //estadosVMDomicilio = domicilioEstado,
                                         //municipiosVMDomicilio= domicilioMunicipio,
                                     };

            #endregion

            #region -JoinTables null-
            ViewData["joinTablesDomSec"] = from personaTable in personaVM
                                           join domicilio in domicilioVM on persona.IdPersona equals domicilio.PersonaIdPersona
                                           join domicilioSec in domicilioSecundarioVM.DefaultIfEmpty() on domicilio.IdDomicilio equals domicilioSec.IdDomicilio
                                           where personaTable.IdPersona == id
                                           select new PersonaViewModel
                                           {
                                               domicilioSecundarioVM = domicilioSec
                                           };

            ViewData["joinTablesConsumoSustancias"] = from personaTable in personaVM
                                                      join sustancias in consumoSustanciasVM on persona.IdPersona equals sustancias.PersonaIdPersona
                                                      where personaTable.IdPersona == id
                                                      select new PersonaViewModel
                                                      {
                                                          consumoSustanciasVM = sustancias
                                                      };

            ViewData["joinTablesFamiliaresForaneos"] = from personaTable in personaVM
                                                       join familiarForaneo in familiaresForaneosVM on persona.IdPersona equals familiarForaneo.PersonaIdPersona
                                                       where personaTable.IdPersona == id
                                                       select new PersonaViewModel
                                                       {
                                                           familiaresForaneosVM = familiarForaneo
                                                       };

            ViewData["joinTablesFamiliares"] = from personaTable in personaVM
                                               join familiar in asientoFamiliarVM on persona.IdPersona equals familiar.PersonaIdPersona
                                               where personaTable.IdPersona == id && familiar.Tipo == "FAMILIAR"
                                               select new PersonaViewModel
                                               {
                                                   asientoFamiliarVM = familiar
                                               };

            ViewData["joinTablesReferencia"] = from personaTable in personaVM
                                               join referencia in asientoFamiliarVM on persona.IdPersona equals referencia.PersonaIdPersona
                                               where personaTable.IdPersona == id && referencia.Tipo == "REFERENCIA"
                                               select new PersonaViewModel
                                               {
                                                   asientoFamiliarVM = referencia
                                               };


            ViewBag.Referencia = ((ViewData["joinTablesReferencia"] as IEnumerable<scorpioweb.Models.PersonaViewModel>).Count()).ToString();

            ViewBag.Familiar = ((ViewData["joinTablesFamiliares"] as IEnumerable<scorpioweb.Models.PersonaViewModel>).Count()).ToString();
            #endregion


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
            string currentUser = User.Identity.Name;
            for (int i = 0; i < datosConsumo.Length; i++)
            {
                datosSustancias.Add(new List<String> { datosConsumo[i], currentUser });
            }

            return Json(new { success = true, responseText = "Datos Guardados con éxito" });

        }

        public ActionResult guardarFamiliar(string[] datosFamiliar, int tipoGuardado)
        {
            string currentUser = User.Identity.Name;
            if (tipoGuardado == 1)
            {
                for (int i = 0; i < datosFamiliar.Length; i++)
                {
                    datosFamiliares.Add(new List<String> { datosFamiliar[i], currentUser });
                }
            }
            else if (tipoGuardado == 2)
            {
                for (int i = 0; i < datosFamiliar.Length; i++)
                {
                    datosReferencias.Add(new List<String> { datosFamiliar[i], currentUser });
                }
            }


            return Json(new { success = true, responseText = "Datos Guardados con éxito" });

        }

        public ActionResult guardarFamiliarExtranjero(string[] datosFE)
        {
            string currentUser = User.Identity.Name;
            for (int i = 0; i < datosFE.Length; i++)
            {
                datosFamiliaresExtranjero.Add(new List<String> { datosFE[i], currentUser });
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
        [Authorize(Roles = "AdminMCSCP, SupervisorMCSCP, Masteradmin")]
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

        public string generaEstado(string id)
        {
            string estado = "";

            if (id == "0")
            {
                estado = "SIN ESTADO";
            }
            else
            {
                if (!String.IsNullOrEmpty(id))
                {
                    List<Estados> estados = _context.Estados.ToList();
                    estado = (estados.FirstOrDefault(x => x.Id == Int32.Parse(id)).Estado).ToUpper();
                }
            }
            return estado;
        }

        public string generaMunicipio(string id)
        {
            string municipio = "";

            if (id == "0")
            {
                municipio = "SIN MUNICIPIO";
            }
            else
            {
                if (!String.IsNullOrEmpty(id))
                {
                    List<Municipios> municipios = _context.Municipios.ToList();
                    municipio = (municipios.FirstOrDefault(x => x.Id == Int32.Parse(id)).Municipio).ToUpper();
                }
            }            
            return municipio;
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
            string motivoDS, string tipoDomicilioDS, string calleDS, string noDS, string nombreCFDS, string paisDDS, string estadoDDS, string municipioDDS, string temporalidadDS,
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
            string currentUser = User.Identity.Name;


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
                persona.Lnestado = generaEstado(lnEstado);
                persona.Lnmunicipio = generaMunicipio(lnMunicipio);
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
                domicilio.Estado = generaEstado(estadoD);
                domicilio.Municipio = generaMunicipio(municipioD);
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
                domiciliosecundario.Estado = generaEstado(estadoDDS);
                domiciliosecundario.Municipio = generaMunicipio(municipioDDS);
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
                for (int i = 0; i < datosSustancias.Count; i = i + 5)
                {
                    if (datosSustancias[i][1] == currentUser)
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

                for (int i = 0; i < datosSustancias.Count; i++)
                {
                    if (datosSustancias[i][1] == currentUser)
                    {
                        datosSustancias.RemoveAt(i);
                        i--;
                    }
                }
                #endregion

                #region -AsientoFamiliar-
                for (int i = 0; i < datosFamiliares.Count; i = i + 13)
                {
                    if (datosFamiliares[i][1] == currentUser)
                    {
                        asientoFamiliar.Nombre = normaliza(datosFamiliares[i][0]);
                        asientoFamiliar.Relacion = datosFamiliares[i + 1][0];
                        asientoFamiliar.Edad = Int32.Parse(datosFamiliares[i + 2][0]);
                        asientoFamiliar.Sexo = datosFamiliares[i + 3][0];
                        asientoFamiliar.Dependencia = datosFamiliares[i + 4][0];
                        asientoFamiliar.DependenciaExplica = normaliza(datosFamiliares[i + 5][0]);
                        asientoFamiliar.VivenJuntos = datosFamiliares[i + 6][0];
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
                    if (datosReferencias[i][1] == currentUser)
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



                for (int i = 0; i < datosReferencias.Count; i++)
                {
                    if (datosReferencias[i][1] == currentUser)
                    {
                        datosReferencias.RemoveAt(i);
                        i--;
                    }
                }

                #endregion

                #region -Familiares Extranjero-
                for (int i = 0; i < datosFamiliaresExtranjero.Count; i = i + 12)
                {
                    if (datosFamiliaresExtranjero[i][1] == currentUser)
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

                for (int i = 0; i < datosFamiliaresExtranjero.Count; i++)
                {
                    if (datosFamiliaresExtranjero[i][1] == currentUser)
                    {
                        datosFamiliaresExtranjero.RemoveAt(i);
                        i--;
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

        #region -Reportes-
        public ActionResult ReportePersona()
        {
            return View();
        }

        public ActionResult Imprimir(int id)
        {
            var PDFResult = new ViewAsPdf("Details")
            {
                FileName = "Reporte.PDF"
            };
            return PDFResult;
        }
        #endregion

        #region -Edicion-        

        public async Task<IActionResult> MenuEdicion(int? id)
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


        #region -Edita Datos Generales-
        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona.SingleOrDefaultAsync(m => m.IdPersona == id);

            List<Estados> listaEstados = new List<Estados>();
            listaEstados = (from table in _context.Estados
                            select table).ToList();

            listaEstados.Insert(0, new Estados { Id = 0, Estado = "Selecciona" });
            ViewBag.ListadoEstados = listaEstados;

            //ViewBag.Countries = new SelectList(countries.GetCountries(), "id", "countryName", "82");

            //ViewBag.DDEstados = new SelectList(listaEstados, "Id", "Estados", persona.Lnestado);
            ViewBag.idEstado = persona.Lnestado;

            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPersona,Nombre,Paterno,Materno,Alias,Genero,Edad,Fnacimiento,Lnpais,Lnestado,Lnmunicipio,Lnlocalidad,EstadoCivil,Duracion,OtroIdioma,EspecifiqueIdioma,DatosGeneralescol,LeerEscribir,Traductor,EspecifiqueTraductor,TelefonoFijo,Celular,Hijos,Nhijos,NpersonasVive,Propiedades,Curp,ConsumoSustancias,UltimaActualización,Supervisor")] Persona persona)
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
        #endregion

        #region -Edita Domicilio-
        public async Task<IActionResult> EditDomicilio(string nombre, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Nombre"] = nombre;
            var domicilio = await _context.Domicilio.SingleOrDefaultAsync(m => m.PersonaIdPersona == id);
            if (domicilio == null)
            {
                return NotFound();
            }
            return View(domicilio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDomicilio(int id, [Bind("IdDomicilio,TipoDomicilio,Calle,No,TipoUbicacion,NombreCf,Pais,Estado,Municipio,Temporalidad,ResidenciaHabitual,Cp,Referencias,Horario,DomcilioSecundario,Observaciones,PersonaIdPersona")] Domicilio domicilio)
        {
            if (id != domicilio.PersonaIdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(domicilio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(domicilio.PersonaIdPersona))
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
            return View(domicilio);
        }
        #endregion

        #region -Edita Escolaridad-
        public async Task<IActionResult> EditEscolaridad(string nombre, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Nombre"] = nombre;
            var estudios = await _context.Estudios.SingleOrDefaultAsync(m => m.PersonaIdPersona == id);
            if (estudios == null)
            {
                return NotFound();
            }
            return View(estudios);
        }

        // POST: Estudios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEscolaridad(int id, [Bind("IdEstudios,Estudia,GradoEstudios,InstitucionE,Horario,Direccion,Telefono,Observaciones,PersonaIdPersona")] Estudios estudios)
        {
            if (id != estudios.PersonaIdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudios);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(estudios.PersonaIdPersona))
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
            return View(estudios);
        }
        #endregion

        #region -Edita Trabajo-
        public async Task<IActionResult> EditTrabajo(string nombre, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Nombre"] = nombre;
            var trabajo = await _context.Trabajo.SingleOrDefaultAsync(m => m.PersonaIdPersona == id);
            if (trabajo == null)
            {
                return NotFound();
            }
            return View(trabajo);
        }

        // POST: Trabajoes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTrabajo(int id, [Bind("IdTrabajo,Trabaja,TipoOcupacion,Puesto,EmpledorJefe,EnteradoProceso,SePuedeEnterar,TiempoTrabajano,Salario,TemporalidadSalario,Direccion,Horario,Telefono,Observaciones,PersonaIdPersona")] Trabajo trabajo)
        {
            if (id != trabajo.PersonaIdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trabajo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(trabajo.PersonaIdPersona))
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
            return View(trabajo);
        }
        #endregion

        #region -Edita Actividades Sociales-
        public async Task<IActionResult> EditActividadesSociales(string nombre, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Nombre"] = nombre;
            var actividadsocial = await _context.Actividadsocial.SingleOrDefaultAsync(m => m.PersonaIdPersona == id);
            if (actividadsocial == null)
            {
                return NotFound();
            }
            return View(actividadsocial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActividadesSociales(int id, [Bind("IdActividadSocial,TipoActividad,Horario,Lugar,Telefono,SePuedeEnterar,Referencia,Observaciones,PersonaIdPersona")] Actividadsocial actividadsocial)
        {
            if (id != actividadsocial.PersonaIdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actividadsocial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(actividadsocial.PersonaIdPersona))
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
            return View(actividadsocial);
        }
        #endregion

        #region -Edita Abandono Estado-
        public async Task<IActionResult> EditAbandonoEstado(string nombre, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Nombre"] = nombre;
            var abandonoestado = await _context.Abandonoestado.SingleOrDefaultAsync(m => m.PersonaIdPersona == id);
            if (abandonoestado == null)
            {
                return NotFound();
            }
            return View(abandonoestado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAbandonoEstado(int id, [Bind("IdAbandonoEstado,VividoFuera,LugaresVivido,TiempoVivido,MotivoVivido,ViajaHabitual,LugaresViaje,TiempoViaje,MotivoViaje,DocumentacionSalirPais,Pasaporte,Visa,FamiliaresFuera,Cuantos,PersonaIdPersona")] Abandonoestado abandonoestado)
        {
            if (id != abandonoestado.PersonaIdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(abandonoestado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(abandonoestado.PersonaIdPersona))
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
            return View(abandonoestado);
        }
        #endregion

        #region -Editar Salud-
        public async Task<IActionResult> EditSalud(string nombre, int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Nombre"] = nombre;
            var saludfisica = await _context.Saludfisica.SingleOrDefaultAsync(m => m.PersonaIdPersona == id);
            if (saludfisica == null)
            {
                return NotFound();
            }
            return View(saludfisica);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSalud(int id, [Bind("IdSaludFisica,Enfermedad,EspecifiqueEnfermedad,EmbarazoLactancia,Tiempo,Tratamiento,Discapacidad,EspecifiqueDiscapacidad,ServicioMedico,EspecifiqueServicioMedico,InstitucionServicioMedico,Observaciones,PersonaIdPersona")] Saludfisica saludfisica)
        {
            if (id != saludfisica.PersonaIdPersona)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(saludfisica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(saludfisica.PersonaIdPersona))
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
            return View(saludfisica);
        }
        #endregion

        #endregion

        #region -Borrar-
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
        #endregion


        private bool PersonaExists(int id)
        {
            return _context.Persona.Any(e => e.IdPersona == id);
        }
    }
}
