using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class InconsistenciaSeguridadDto
    {
        public string Novedad { get; set; }
        public Guid InconsistenciaId { get; set; }
        public string ListaInconsistenciaId { get; set; }
        public string CausalInterrupcionId { get; set; }
        public int GrupoInconsistencia { get; set; }
        public string Informe { get; set; }
        public bool Subsanado { get; set; }
        public string InconsistenciaId_nuevo { get; set; }
        public string CausalInterrupcionId_nuevo { get; set; }
        public string Informe_nuevo { get; set; }
        public Guid Solicitudid { get; set; }
        public DateTime Fecha_subsanacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Guid solicitudTareaId { get; set;  }


    }
}
