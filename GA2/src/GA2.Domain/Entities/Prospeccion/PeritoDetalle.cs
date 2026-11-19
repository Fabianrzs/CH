using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Prospeccion
{
    public class PeritoDetalle
    {
        public Guid PER_ID { get; set; }
        public string PER_OBSERVACION { get; set; }
        public DateTime PER_FECHA_REGISTRO { get; set; }
        public Guid PER_CREADO_POR { get; set; }
        public Guid SOC_ID { get; set; }
        public Guid PER_PERITO_ID { get; set; }
    }
}
