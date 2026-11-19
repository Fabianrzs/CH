using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.VerificacionDerechos.CuentasAVAC
{
    public class ObtenerCuentasAVACDto
    {

        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; }
        public int CliId { get; set; }
        public int TctId { get; set; }
        public int EctId { get; set; }
        public DateTime CtaFechaCreacion { get; set; }
        public decimal CtaSaldo { get; set; }

        public DateTime? AVAC { get; set; }
        public decimal ValorAVAC { get; set; }
        public decimal SaldoAnualAVAC { get; set; }
        public decimal InteresesAVAC { get; set; }
        public decimal MovimientosMensualAVAC { get; set; }


        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }

    }
}
