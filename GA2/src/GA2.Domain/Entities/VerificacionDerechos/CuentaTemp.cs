using System;

namespace GA2.Domain.Entities.VerificacionDerechos
{
    public class CuentaTemp
    {
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public int EctId { get; set; }
        public DateTime CtaFechaCreacion { get; set; }
        public decimal? CtaSaldo { get; set; }
        public DateTime? AVAC { get; set; }
        public decimal? ValorAVAC { get; set; }
        public decimal? SaldoAnualAVAC { get; set; }
        public decimal? MovimientosMensualAVAC { get; set; }
        public DateTime? AVAV { get; set; }
        public decimal? ValorAVAV { get; set; }
        public decimal? SaldoAnualAVAV { get; set; }
        public decimal? MovimientosMensualAVAV { get; set; }
        public DateTime? AVC { get; set; }
        public decimal? ValorAVC { get; set; }
        public decimal? SaldoAnualAVC { get; set; }
        public decimal? MovimientosMensualAVC { get; set; }
        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }
    }

}