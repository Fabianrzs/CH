using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class NovedadesAfiliado
    {
        public int NVA_ID { get; set; }
	    public int TID_ID { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string COD_MILITAR { get; set; }
        public int CNV_ID { get; set; }
        public DateTime NVA_FECHA_INICIO { get; set; }
        public DateTime NVA_FECHA_FIN { get; set; }
        public decimal NVA_PORCENTAJE { get; set; }
        public decimal NVA_VALOR { get; set; }
        public bool NVA_ES_ALTA { get; set; }
        public bool NVA_ES_BAJA { get; set; }
        public string NVA_OBSERVACIONES { get; set; }
        public string? TOKEN { get; set; }
        public int UEJ_ID { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public string CNV_NOMBRE_NOVEDAD { get; set; }
        public string NVD_CODIGO_ALTA { get; set; }
        public string NVD_CODIGO_BAJA { get; set; }
    }
}
