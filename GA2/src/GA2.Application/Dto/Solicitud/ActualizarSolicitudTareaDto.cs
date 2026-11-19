using System;
namespace GA2.Application.Dto
{
    public class ActualizarSolicitudTareaDto
    {
        public Guid solicitudTareaId { get; set; }
        public int estadoSolicitudTarea { get; set; }
        public string observaciones { get; set; }
    }
}

