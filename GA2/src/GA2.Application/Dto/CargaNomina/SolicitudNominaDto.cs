using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.CargaNomina
{
    public class SolicitudNominaDto
    {
        [Required]
        public int DocumentoId { get; set; }
        public Guid SolicitudId { get; set; }
        public int NumeroSolicitud { get; set; }
        public int EstadoSolicitud { get; set; }
        public Boolean Estado { get; set; }
        public DateTime FechaInicialDocumento { get; set; }
        public DateTime FechaFinalDocumento { get; set; }
        public string Observacion { get; set; }
        public Guid ProcesadoPor { get; set; }
        public Guid AutorizadoPor { get; set; }
        public Guid AprobadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string NumeroRadicado { get; set; }
        public decimal Monto { get; set; }
        public int Periodo { get; set; }
        public long Registros { get; set; }
        public string TipoArchivo { get; set; }
        public string Nombre { get; set; }
    }
}
