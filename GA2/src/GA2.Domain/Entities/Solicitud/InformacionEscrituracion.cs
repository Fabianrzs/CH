using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class InformacionEscrituracion
    {
        public string? NumeroEscritura { get; set; }
        public DateTime? FechaEscrituracion { get; set; }
        public string? Notaria { get; set; }
        public DateTime FechaEscrituracionRegistro { get; set; }
    }
}
