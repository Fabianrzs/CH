using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class Entrevista
    {
        public Guid EFS_ID { get; set; }
        public Guid EFS_ID_SOLICITUD { get; set; }

        public string EFS_VIVIENDA_PROPIA { get; set; }
        public string EFS_TIPO_VIVIENDA { get; set; }
        public int EFS_HABITANTE { get; set; }
        public string EFS_INFORMATIVO_ADMIN { get; set; }
        public string EFS_CONCEPTO_PSICOSOCIAL { get; set; }
        public string? EFS_OBSERVACION { get; set; }
        public int? EFS_ESTADO { get; set; }
        public int? EFS_ACTIVIDAD { get; set; }
        public Guid UserId { get; set; }

    }
}
