using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ConvocatoriaResponse
    {
        public int ID_CONVOCATORIA { get; set; }
        public DateTime FECHA_APERTURA { get; set; }
        public DateTime FECHA_CIERRE { get; set; }
        public DateTime FECHA_PUBLICACION { get; set; }
        public long NUMERO_BENEFICIARIO { get; set; }
        public long NUMERO_RESOLUCION { get; set; }
        public long NUMERO_CONVOCATORIA { get; set; }
        public long NUMERO_DIARIOFICIAL { get; set; }
        public string NOMBRE_ESTADO { get; set; }
        public int CON_DELETE { get; set; }
        public DateTime FechaResolucion { get; set; }
    }
}
