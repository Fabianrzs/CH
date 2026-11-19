using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class CuentasAfectadasSolicitud
    {

        public Guid CAC_ID { get; set; }
        public Guid CSA_ID { get; set; }
        public int CTC_ID { get; set; }
        public string CTC_DESCRIPCION { get; set; }
        public Guid SOL_ID_FK { get; set; }
        public int CLI_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public decimal CAC_SALDO { get; set; }
        public decimal CAC_VALOR_A_RETIRAR { get; set; }
        public int NUMERO_CUENTA { get; set; }
        public string DESCRIPCION_CUENTA { get; set; }
        public decimal CAC_VALOR_DESCONTADO { get; set; }
        public decimal TOTAL_DESCONTADO { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }
        public decimal TOTAL_DESCONTADO_CUENTA { get; set; }
        public int TER_TIPO_CUENTA { get; set; }
        public Guid ENT_ID_FK  { get; set; }

    }
}
