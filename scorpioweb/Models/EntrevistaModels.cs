using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scorpioweb.Models
{
    public class EntrevistaModels
    {
        public IEnumerable <Persona> Persona { get; set; }
        public IEnumerable <Proceso> Proceso { get; set; }
    }
}
