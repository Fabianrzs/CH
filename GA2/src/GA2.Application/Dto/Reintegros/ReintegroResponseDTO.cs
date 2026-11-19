using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ReintegroResponseDTO
    {
        public Guid Id_Reintegro { get; set; }
        public string Unidad_Ejecutora { get; set; }
        public string EstadoAfiliao { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal ValorSueldo { get; set; }
        public decimal ValorSueldoPorcentaje { get; set; }
        public int NumeroSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public string ModalidadReintegro { get; set; }
        public string Numero_Radicado { get; set; }
        public string TareaNombre { get; set; }
        public int NumerSolicitud { get; set; }
    }
}
