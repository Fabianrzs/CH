using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class HistorialCambiosDto
    {
        public int IdEntidadCambio { get; set; }
        public Guid IdEntidad { get; set; }
        public string Pestaña { get; set; }
        public string Campo { get; set; }
        public string VlrAnterior { get; set; }
        public string NombreUsuario { get; set; }
        public string VlrNuevo { get; set; }
        public Guid IdUsuario { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

    public class HistorialCambiosIntervinienteDto
    {
        public int IdCambio { get; set; }
        public Guid idInterviniente { get; set; }
        public string Pestaña { get; set; }
        public string Campo { get; set; }
        public string VlrAnterior { get; set; }
        public string NombreUsuario { get; set; }
        public string VlrNuevo { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string Modulo { get; set; }
    }

}
