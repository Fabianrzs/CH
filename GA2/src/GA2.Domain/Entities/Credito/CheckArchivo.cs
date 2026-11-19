using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class CheckArchivo
    {
        public Guid SAC_ID { get; set; }
        public string SAC_NOMBRE_ARCHIVO { get; set; }
        public DateTime SAC_FECHA_CHECK { get; set; }
        public Guid SOC_ID_FK { get; set; }
    }
}
