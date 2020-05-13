using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scorpioweb.Models
{
    public class PersonaViewModel
    {
        public Persona personaVM { get; set; }
        public Domicilio domicilioVM { get; set; }
        public Estudios estudiosVM { get; set; }
        public Estados estadosVMPersona { get; set; }
        public Municipios municipiosVMPersona { get; set; }
        public Estados estadosVMDomicilio { get; set; }
        public Municipios municipiosVMDomicilio { get; set; }
        public Domiciliosecundario domicilioSecundarioVM { get; set; }
        public Estados estadosVMDomicilioSec { get; set; }
        public Municipios municipiosVMDomicilioSec { get; set; }
    }
}
