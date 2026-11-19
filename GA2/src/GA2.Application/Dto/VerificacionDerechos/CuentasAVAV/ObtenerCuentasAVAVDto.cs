using System;

namespace GA2.Application.Dto.VerificacionDerechos.CuentasAVAV
{
    public class ObtenerCuentasAVAVDto
    {

        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public int EctId { get; set; }
        public DateTime CtaFechaCreacion { get; set; }
        public decimal CtaSaldo { get; set; }

        public DateTime? AVAV { get; set; }
        public decimal ValorAVAV { get; set; }
        public decimal SaldoAnualAVAV { get; set; }
        public decimal InteresesAVAV { get; set; }
        public decimal MovimientosMensualAVAV { get; set; }


        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }

    }

    public class ObtenerCuentaAVAVResponseDto
    {
        public string Identificacion { get; set; }
        public int IdCliente { get; set; }
        public int CuentaId { get; set; }
        public int CuentaNumero { get; set; }
        public int ConceptoId { get; set; }
    }
}
