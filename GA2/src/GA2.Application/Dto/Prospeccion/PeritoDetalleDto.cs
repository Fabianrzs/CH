using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Prospeccion
{
    public class PeritoDetalleDto
    {
        public string Descripcion { get; set; }
        public Guid SOC_ID { get; set; }

        public Guid PER_ID { get; set; }
    }
}
