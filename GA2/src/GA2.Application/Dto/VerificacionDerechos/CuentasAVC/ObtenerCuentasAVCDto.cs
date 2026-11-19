using System;

namespace GA2.Application.Dto.VerificacionDerechos.CuentasAVC
{
    public class ObtenerCuentasAVCDto
    {

        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public int EctId { get; set; }
        public DateTime CtaFechaCreacion { get; set; }
        public decimal CtaSaldo { get; set; }

        public DateTime? AVC { get; set; }
        public decimal ValorAVC { get; set; }
        public decimal SaldoAnualAVC { get; set; }
        public decimal InteresesAVC { get; set; }
        public decimal MovimientosMensualAVC { get; set; }


        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }

    }
}
