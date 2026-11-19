using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CargaNomina
{
    public class EncabezadoArchivosNomina
    {
        public int EAN_ID { get; set; } 
        public int DCT_ID { get; set; }
        public int EAN_CONSECUTIVO { get; set; }
        public int TAR_ID { get; set; }
        public string EAN_NOMBRE_UNIDAD_EJECUTORA { get; set; } = string.Empty;
        public int UEJ_ID { get; set; }
        public string EAN_PERIODO_APORTE { get; set; } = string.Empty;
    }
}
