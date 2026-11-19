using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class PreaprobacionesDto
    {
        
        public string NumeroDcocumento { get; set; }
        public decimal ValorPrestamo { get; set; }
        public DateTime FechaSimulacion { get; set; }
        public int NumeroPreaprobado { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }

    }

    public class RequestPreaprobacionesDto
    {
        public string? NumeroDocumento { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaSimulacion { get; set; }
        public DateTime? FechaSimulacionFin { get; set; }

    }

}
