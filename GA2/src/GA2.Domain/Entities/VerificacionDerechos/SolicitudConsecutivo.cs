using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.VerificacionDerechos
{
    public class SolicitudConsecutivo
    {

        public string CNS_ID { get; set; }
        public string CNS_DESCRIPCION { get; set; }
        public long CNS_ULTIMO_NUMERO { get; set; }
        public int CNS_ESTADO { get; set; }
        public Guid CNS_CREADO_POR { get; set; }
        public DateTime CNS_FECHA_CREACION { get; set; }
        public Guid CNS_ACTUALIZADO_POR { get; set; }
        public DateTime CNS_FECHA_ACTUALIZA { get; set; }



    }
}
