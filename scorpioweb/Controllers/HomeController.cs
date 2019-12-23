using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using scorpioweb.Models;
using Microsoft.AspNetCore.Authorization;

namespace scorpioweb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult QR()
        {
            ViewData["Message"] = "Your QR page.";

            return View();
        }

        public IActionResult Generate(string consecutivo, string paterno, string materno, string nombre)
        {
            ViewBag.paterno = paterno;
            ViewBag.materno = materno;
            ViewBag.nombre = nombre;


            return View("QR");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
