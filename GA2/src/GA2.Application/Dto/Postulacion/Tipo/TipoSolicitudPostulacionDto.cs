using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class TipoSolicitudPostulacionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaInsercion { get; set; }
        public int Estado { get; set; }
    }
}
