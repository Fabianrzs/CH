using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CargaNomina
{
    public class CuerpoArchivosNomina
    {
        public int CAN_ID { get; set; } 
        public int DCT_ID { get; set; }
        public int CAN_CONSECUTIVO { get; set; }
        public int CAN_DIGITO_FUERZA { get; set; }
        public int UEJ_ID { get; set; }
        public int TID_ID { get; set; }
        public string CAN_IDENTIFICACION { get; set; } = string.Empty;
        public string CAN_COD_MILITAR { get; set; } = string.Empty;
        public string CAN_PRIMER_NOMBRE { get; set; } = string.Empty;
        public string CAN_SEGUNDO_NOMBRE { get; set; } = string.Empty;
        public string CAN_PRIMER_APELLIDO { get; set; } = string.Empty;
        public string CAN_SEGUNDO_APELLIDO { get; set; } = string.Empty;
        public int CAN_EMBARGO { get; set; }
        public decimal CAN_BASE_CALCULO { get; set; }
        public decimal? CAN_VALOR { get; set; }
        public DateTime CAN_PERIODO { get; set; }
        public int GRD_ID { get; set; }
        public string CAN_UNIDAD_OPERATIVA { get; set; } = string.Empty;
    }
}
