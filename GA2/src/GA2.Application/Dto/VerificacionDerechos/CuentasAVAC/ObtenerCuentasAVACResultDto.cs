using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.VerificacionDerechos.CuentasAVAC
{
    public class ObtenerCuentasAVACResultDto
    {
        public long Id { get; set; }
        public int CtaId { get; set; }
        public int CtaNumero { get; set; } 
        public int EctId { get; set; }
        public int PeriodoAnio { get; set; }
        public int PeriodoMes { get; set; }
        public decimal CtaSaldo { get; set; }


        public decimal ValorAVAC { get; set; }
        public decimal SaldoAnualAVAC { get; set; }        
        public decimal MovimientosMensualAVAC { get; set; }
        public decimal InteresesAVAC { get; set; }
        public decimal InteresesNoCausadoAVAC { get; set; }
        public decimal InteresesCausadoAVAC { get; set; }
        public decimal SaldoAnteriorAVAC { get; set; }
        public decimal SaldoFinalAVAC { get; set; }


        public int CliId { get; set; }
        public string Identificacion { get; set; }
        public int UnidadEjecutora { get; set; }

        public long NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; }

        public decimal InteresNoAbonadosSaldoInsuficienteAVAC { get; set; }
    }
}
