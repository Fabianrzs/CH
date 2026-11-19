using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ActualizarSolicitudTarea
    {
        public Guid SLT_ID { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public string SLT_OBSERVACION { get; set; } = null;
        public string? NOMBRE_COMPLETO { get; set; } = null;
        public string? NUM_CEDULA { get; set; } = null;




    }
}
