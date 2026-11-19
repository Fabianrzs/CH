using System;

namespace GA2.Application.Dto
{
    public class RestitucionInmuebleCarteraDto
    {
        public int? CodigoRestitucion { get; set; } = null;
        public int TipoNovedad { get; set; }
        public int TipoDpcumento { get; set; }
        public string NumeroDocumento { get; set; }
        public Guid IdCredito { get; set; }
        public long? NumeroCredito { get; set; }
        public int? IdTipoCredito { get; set; }
        public DateTime FechaNovedad { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public decimal Capital { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal SeguroHogar { get; set; }
        public decimal InteresCorriente { get; set; }
        public decimal InteresMora { get; set; }
        public decimal CapitalRemanente { get; set; }
        public decimal InteresRemanente { get; set; }
        public decimal TotalDeuda { get; set; }
        public decimal ValorPagado { get; set; }
        public decimal ValorRestante { get; set; }
    }
}
