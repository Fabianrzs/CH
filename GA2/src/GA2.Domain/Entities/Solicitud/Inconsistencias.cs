using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class Inconsistencias
    {
      
        // inconsistencia
        public int? ID { get; set; }
        public Guid? SLT_TAREA { get; set; }
        public Guid? IdSolicitud { get; set; }
        public string? NOMBRE_REGLA { get; set; }
        public Guid? CREADORPOR { get; set; }
        public DateTime? FECHACREACION { get; set; }
        public string? TRA_NOMBRE { get; set; }
        public string? PUBLICA { get; set; }

    }
}
