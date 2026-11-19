using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.CargueNonima.CargaNomina
{
    public class ArchivosConError
    {
        public int DCT_ID { get; set; }
        public string TIPO_ERROR { get; set; }
        public int UEJ_ID { get; set; }

        //ENCABEZADO
        public int EAN_CONSECUTIVO { get; set; }
        public int TAR_ID { get; set; }
        public string TAR_DESCRIPCION { get; set; }
        public string EAN_NOMBRE_UNIDAD_EJECUTORA { get; set; }
        public string EAN_PERIODO_APORTE { get; set; }

        //CUERPO
        public int CAN_CONSECUTIVO { get; set; }
        public int CAN_DIGITO_FUERZA { get; set; }
        public int TID_ID { get; set; }
        public string CAN_IDENTIFICACION { get; set; }
        public string CAN_COD_MILITAR { get; set; }
        public string CAN_PRIMER_NOMBRE { get; set; }
        public string CAN_SEGUNDO_NOMBRE { get; set; }
        public string CAN_PRIMER_APELLIDO { get; set; }
        public string CAN_SEGUNDO_APELLIDO { get; set; }
        public int CAN_EMBARGO { get; set; }
        public int CAN_BASE_CALCULO { get; set; }
        public int CAN_VALOR { get; set; }
        public string CAN_PERIODO { get; set; }
        public int GRD_ID  { get; set; }
        public string CAN_UNIDAD_OPERATIVA { get; set; }

        //FIN
        public int FAN_CONSECUTIVO { get; set; }
        public string FAN_INDICADOR_CONTROL { get; set; }
        public string FAN_SUMATORIA_APORTE { get; set; }
        public int FAN_CODIGO_FIN_ARCHIVO { get; set; }
    }
}
