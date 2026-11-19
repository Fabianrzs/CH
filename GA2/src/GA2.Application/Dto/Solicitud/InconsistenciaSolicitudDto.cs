using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class InconsistenciaSolicitudDto
    {
        public string Novedad { get; set; }
        public string InconsistenciaId { get; set; }
        public string InconsistenciaDescripcion { get; set; }
        public string ListaInconsistenciaId { get; set; }
        public string CausalInterrupcionId { get; set; }
        public int GrupoInconsistencia { get; set; }
        public Guid? HerramientaId { get; set; }
        public Guid? PuntoAtencionId { get; set; }
        public Guid? SolicitudId { get; set; }
        public string Informe { get; set; }
        public Guid TecnicoId { get; set; }
        public string ObjetoEstudio { get; set; }
        public string Analisis { get; set; }
        public string Observacion { get; set; }
        public bool Estado { get; set; }
        public Guid? CreadoPor { get; set; }
        public Guid? ModificadoPor { get; set; }
        public DateTime Fecha_Subsanacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string RadicadoSolicitud { get; set; }
        public int IdCliente { get; set; }
        public Guid? UserId { get; set; }
        public bool Subsanado { get; set; }
        public string publica { get; set; }
        public Guid solicitudTareaId { get; set; }
    }
}
