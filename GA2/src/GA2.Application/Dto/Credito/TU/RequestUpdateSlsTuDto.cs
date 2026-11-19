using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.TU
{
    public class RequestUpdateSlsTuDto
    {
        public Guid Sls_Id { get; set; }
        public DateTime Fecha_Reporte_TU { get; set; }
        public string xml { get; set; }
    }
}
