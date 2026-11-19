using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class CalificacionResponse
    {
        public Guid CalificacionId { get; set; }
        public Guid SolicitudId { get; set; }
        public string UnidadEjecutora { get; set; }
        public string TipoImputabilidad { get; set; }
        public decimal ImputabilidadAlServicio { get; set; }
        public decimal PorcentajePostulacion { get; set; }
        public decimal Hecho { get; set; }
        public decimal Puntaje { get; set; }
        public string DisfrutePension { get; set; }
        public string DatosResolucion { get; set; }
        public int EstadoId { get; set; }
        public int Actividad { get; set; }
        public string? Observacion { get; set; }
        public string NombreEstado { get; set; }
        public string NombreActividad { get; set; }
        public string HechoDescripcion { get; set; }
        public int PorcentajeDisminucion { get; set; }
        public string? ImputabilidadServicioDescripcion { get; set; }
        public string Descripcion { get; set; }
    }
}
