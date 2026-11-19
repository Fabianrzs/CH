using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Reportes
{
    public class ReporteGeneralMora
    {
        public String NUMERO_RADICACION { get; set; }

        public String ESTADO { get; set; }

        public int CODIGO_CLIENTE { get; set; }

        public String NOMBRE_CLIENTE { get; set; }

        public int CODIGO_EMPRESA { get; set; }

        public String NOMBRE_EMPRESA { get; set; }

        public Double SALDO { get; set; }

        public Double CUOTA { get; set; }

        public int COD_PERIOCIDAD { get; set; }

        public String NAME_PERIOCIDAD { get; set; }

        public String TIPO_RECAUDO { get; set; }

        public String CUOTA_VENCIDA { get; set; }

        public DateTime VENCIMIENTO_PROXIMO_PAGO { get; set; }

        public DateTime ULTIMO_PAGO { get; set; }

        public int VENCIMIENTO_ULTIMO_PAGO { get; set; }

        public Double VALOR_ULTIMO_PAGO { get; set; }

        public String IDENTIFICACION { get; set; }

    }
}
