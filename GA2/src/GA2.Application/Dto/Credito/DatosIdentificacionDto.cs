using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class DatosIdentificacionDto
    {
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public Guid USR_ID { get; set; }
        public Guid SLS_ID { get; set; }
    }
}
