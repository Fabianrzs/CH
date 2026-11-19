using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class Reintegro
    {
        public Guid Id_Reintegro { get; set; }
        public string Numero_Radicado { get; set; }
        public int Id_Unidad_Ejecutora { get; set; }
        public string Unidad_Ejecutora { get; set; }
        public int Id_Tipo_Reintegro { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid UsuarioCreacion { get; set; }
        public bool Estado { get; set; }
        public string? Observacion { get; set; }
        public int NumeroSolicitud { get; set; }

    }
}
