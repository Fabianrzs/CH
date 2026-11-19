
namespace GA2.Application.Dto.VerificacionDerechos.CuentasAVC
{
    public class ObtenerCuentasAVCResultDto
    {

        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int EctId { get; set; }
        public int PeriodoAnio { get; set; }
        public int PeriodoMes { get; set; }
        public decimal CtaSaldo { get; set; }


        public decimal ValorAVC { get; set; }
        public decimal SaldoAnualAVC { get; set; }
        public decimal MovimientosMensualAVC { get; set; }
        public decimal InteresesAVC { get; set; }
        public decimal InteresesNoCausadoAVC { get; set; }
        public decimal InteresesCausadoAVC { get; set; }
        public decimal SaldoAnteriorAVC { get; set; }
        public decimal SaldoFinalAVC { get; set; }


        public int CliId { get; set; }
        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }

        public long NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; }

        public decimal InteresNoAbonadosSaldoInsuficienteAVC { get; set; }

    }
}
