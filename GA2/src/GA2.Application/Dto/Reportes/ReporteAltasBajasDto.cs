using System;

namespace GA2.Application.Dto.Reportes
{
    public class ReporteAltasBajasDto
    {
        public string Identificacion { get; set; }
        public string NombreCompleto { get; set; }
        public decimal ValorCuota { get; set; }
        public string NumeroCredito { get; set; }
        public string Tipo { get; set; }
        public DateTime FechaDocumentoSoporte { get; set; }
        public string Modalidad { get; set; }
        public string CodigoDescuento { get; set; }
        public string FechaReporte { get; set; }
        public decimal MontoDesembolsado { get; set; }
        public string CodigoMilitar { get; set; }
        public string NumeroClienteCygnus { get; set; }
        public string Vacio { get; set; }
        public string FechaInicioCuota { get; set; }
        public string FechaFinCuota { get; set; }
        public string UnidadEjecutora { get; set; }
        public int NumeroCuotas { get; set; }
        public string Grado { get; set; }
        public string Concepto { get; set; }
    }
}
