using System;

namespace GA2.Domain.Entities
{
    public class AnularSolicitudTarea
    {
        public Guid SLT_ID { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public int SOL_ESTADO_SOLICITUD { get; set; }
        public string? NOMBRE_COMPLETO { get; set; }
        public string? NUM_CEDULA { get; set; }
    }
}

