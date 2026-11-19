using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Reportes
{
    public  class Settings
    {
        public Guid RPT_ID { get; set; }

        public int RPT_ID_REPORTE { get; set; }

        public String RPT_NOMBRE_REPORTE { get; set; }

        public String RPT_NOMBRE_CORTO_REPORTE { get; set; }
    }
}
