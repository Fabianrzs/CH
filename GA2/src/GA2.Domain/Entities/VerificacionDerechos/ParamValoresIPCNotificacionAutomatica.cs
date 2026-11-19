using System;

namespace GA2.Domain.Entities.VerificacionDerechos
{
    public class ParamValoresIPCNotificacionAutomatica
    {

        public int ID { get; set; }

        public int ANIO { get; set; }

        public int MES { get; set; }

        public DateTime FECHA_ACTUALIZACION { get; set; }

        public Guid SLT_ID { get; set; }

    }
}
