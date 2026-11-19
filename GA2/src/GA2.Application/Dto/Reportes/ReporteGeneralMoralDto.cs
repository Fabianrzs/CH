using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reportes
{
    public class ReporteGeneralMoralDto
    {
        public String NumeroRadicacion { get; set; }
        public String Estado { get; set; }

        public String CodigoCliente { get; set; }

        public String NombreCliente { get; set; }

        public int CodigoEmpresa { get; set; }

        public String Empresa { get; set; }

        public Double SaldoCapital { get; set; }

        public Double ValorCuota { get; set; }

        public int CodigoPeriocidad { get; set; }

        public String Periocidad { get; set; }

        public String TipoRecaudo { get; set; }

        public String CuotaVencida { get; set; }

        public DateTime VencimientoProximoPago { get; set; }

        public DateTime UltimoPago { get; set; }

        public int VencimientoUltimoPago { get; set; }

        public Double ValorUltimoPago { get; set; }

        public String Identificacion { get; set; }

        public byte[] file { get; set; }
    }
}
