using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class RechazarSolicitudTarea
    {
        public Guid SLT_ID { get; set; }
        public int SLT_ESTADO_SOLICITUD { get; set; }
        public int SLT_ESTADO_SOLICITUD_NUEVO { get; set; }
        public int SOL_ESTADO_SOLICITUD { get; set; }
        public string? NOMBRE_COMPLETO { get; set; }
        public string? NUM_CEDULA { get; set; }
        public Guid SOL_SOLICITUD { get; set; }
        public DateTime SOL_FECHA_REGISTRO { get; set; }
    }
}
