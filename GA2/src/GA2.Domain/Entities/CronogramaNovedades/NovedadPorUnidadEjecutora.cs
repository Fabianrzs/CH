using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class NovedadPorUnidadEjecutora
    {
        public int UEJ_ID { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public int CNV_ID { get; set; }
        public string CNV_DESCRIPCION { get; set; }
        public string NVD_CODIGO_ALTA { get; set; }
        public string NVD_CODIGO_BAJA { get; set; }
        public int NVD_NOVEDADES_ALTA { get; set; }
        public int NVD_NOVEDADES_BAJA { get; set; }
        public DateTime NVA_FECHA_INICIO { get; set; }
        public DateTime NVA_FECHA_FIN { get; set; }
    }
}
