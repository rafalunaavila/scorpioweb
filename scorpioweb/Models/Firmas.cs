using System;
using System.Collections.Generic;

namespace scorpioweb.Models
{
    public partial class Firmas
    {
        public int Idfirmas { get; set; }
        public string Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public string Sexo { get; set; }
        public string Libro { get; set; }
        public string Codigo { get; set; }
    }
}
