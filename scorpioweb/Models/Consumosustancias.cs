using System;
using System.Collections.Generic;

namespace scorpioweb.Models
{
    public partial class Consumosustancias
    {
        public int IdConsumoSustancias { get; set; }
        public string Sustancia { get; set; }
        public string Consume { get; set; }
        public string Frecuencia { get; set; }
        public string Cantidad { get; set; }
        public DateTime? UltimoConsumo { get; set; }
        public string Observaciones { get; set; }
        public int PersonaIdPersona { get; set; }
    }
}
