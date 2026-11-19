using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class TipoReintegrosDto
    {
        public int IdTipoReintegro { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificado { get; set; }
    }
}
