using System;
using System.Collections.Generic;

namespace scorpioweb.Models
{
    public partial class Estudios
    {
        public int IdEstudios { get; set; }
        public string Estudia { get; set; }
        public string GradoEstudios { get; set; }
        public string InstitucionE { get; set; }
        public string Horario { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Observaciones { get; set; }
        public int PersonaIdPersona { get; set; }
    }
}
