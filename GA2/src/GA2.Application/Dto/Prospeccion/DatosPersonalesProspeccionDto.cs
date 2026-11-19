using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Prospeccion
{
    public class DatosPersonalesProspeccionDto
    {
        public Guid SLS_ID { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
    }
}
