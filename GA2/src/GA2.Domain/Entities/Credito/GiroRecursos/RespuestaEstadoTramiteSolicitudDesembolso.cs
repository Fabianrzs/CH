using System;

namespace GA2.Domain.Entities
{
    public class RespuestaEstadoTramiteSolicitudDesembolso
    {
        public string ESTADO_ACTIVIDAD { get; set; }
        public Guid ID_SOLICITUD { get; set; }
        public int ID_ESTADO_ACTIVIDAD { get; set; }
        public int NUMERO_SOLICITUD { get; set; }
        public int ID_TIPO_CREDITO { get; set; }
    }
}
