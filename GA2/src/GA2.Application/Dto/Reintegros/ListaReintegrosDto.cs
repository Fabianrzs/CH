using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ListaReintegrosDto
    {
        public Guid Id_Reintegro { get; set; }
        public string Numero_Radicado { get; set; }
        public int Id_Unidad_Ejecutora { get; set; }
        public string Unidad_Ejecutora { get; set; }
        public string TipoReintegro { get; set; }
        public string NumeroDocumento { get; set; }
        public string NombreAfiliado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string ValorSueldo { get; set; }
        public string EstadoReintegro { get; set; }
        public Guid IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Numero_Radicado_Relacionado { get; set; }
    }
}
