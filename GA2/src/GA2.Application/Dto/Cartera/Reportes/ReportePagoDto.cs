using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReportePagoDto
    {
        public Guid IdCredito { get; set; }
        public long NumeroCredito { get; set; }
        public string Identificacion { get; set; }
        public string Afiliado { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public string UnidadEjecutora { get; set; }
        public int IdTipoCredito { get; set; }
        public string TipoCredito { get; set; }
        public string CodigoTipoPago { get; set; }
        public string TipoPago { get; set; }
        public string ComprobanteInterno { get; set; }
        public string ModalidadPago { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public string TipoRecurso { get; set; }
    }
}
