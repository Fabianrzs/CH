using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class Convocatoria
    {
        [Key]
        public int ID_CONVOCATORIA { get; set; }
        public DateTime FECHA_APERTURA { get; set; }
        public DateTime FECHA_CIERRE { get; set; }
        public DateTime FECHA_PUBLICACION { get; set; }
        public long NUMERO_BENEFICIARIO { get; set; }
        public long NUMERO_RESOLUCION { get; set; }
        public long NUMERO_CONVOCATORIA { get; set; }
        public long NUMERO_DIARIOFICIAL { get; set; }
        public int ESTADO_CONVOCATORIA_ID { get; set; }
        public DateTime FECHARESOLUCION { get; set; }

    }
}
