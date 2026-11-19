using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CargaNomina
{
    public class FinArchivosNomina
    {
        public int FAN_ID { get; set; } 
        public int DCT_ID { get; set; }
        public int FAN_CONSECUTIVO { get; set; }
        public string FAN_INDICADOR_CONTROL { get; set; } = string.Empty;
        public string FAN_SUMATORIA_APORTE { get; set; } = string.Empty;
        public int FAN_CODIGO_FIN_ARCHIVO { get; set; }
    }
}
