using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class OpcionTemporalUsuarioDto
    {
        public int IdOpcionTemporalUsuario { get; set; }
        public int IdOpcion { get; set; }
        public Guid IdUsuario { get; set; }
        public string CodigoOpcionTemporalUsuario { get; set; }
        public string NombreOpcionTemporalUsuario { get; set; }
        public string DescripcionOpcionTemporalUsuario { get; set; }
        public DateTime FechaDesdeOpcionTemporalUsuario { get; set; }
        public DateTime FechaHastaOpcionTemporalUsuario { get; set; }
    }
}
