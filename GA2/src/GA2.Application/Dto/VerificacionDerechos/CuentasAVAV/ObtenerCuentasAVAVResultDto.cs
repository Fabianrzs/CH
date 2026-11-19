using System;

namespace GA2.Application.Dto.VerificacionDerechos.CuentasAVAV
{
    public class ObtenerCuentasAVAVResultDto
    {

        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int EctId { get; set; }
        public int PeriodoAnio { get; set; }
        public int PeriodoMes { get; set; }
        public decimal CtaSaldo { get; set; }


        public decimal ValorAVAV { get; set; }
        public decimal SaldoAnualAVAV { get; set; }
        public decimal MovimientosMensualAVAV { get; set; }
        public decimal InteresesAVAV { get; set; }
        public decimal InteresesNoCausadoAVAV { get; set; }
        public decimal InteresesCausadoAVAV { get; set; }
        public decimal SaldoAnteriorAVAV { get; set; }
        public decimal SaldoFinalAVAV { get; set; }


        public int CliId { get; set; }
        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }

        public long NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; }

        public decimal InteresNoAbonadosSaldoInsuficienteAVAV { get; set; }

    }
}
