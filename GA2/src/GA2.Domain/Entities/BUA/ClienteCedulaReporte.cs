using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ClienteCedulaReporte
    {
        public DateTime CLI_FECHA_ALTA { get; set; }
        public DateTime CLI_FECHA_INSCRIPCION { get; set; }

    }
    public class ClienteNomina
    {
        public int CLI_ID { get; set; }
        public int FRZ_ID { get; set; }
        public int UEJ_ID { get; set; }

    }
}
