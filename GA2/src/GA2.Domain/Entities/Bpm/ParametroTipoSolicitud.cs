namespace GA2.Domain.Entities.Bpm
{
    using System;
    using System.Collections.Generic;
    public class ParametroTipoSolicitud
    {
        public Guid PRM_ID { get; set; }
        public int PRM_TIPO_SOLICITUD { get; set; }
        public string PRM_DESCRIPCION { get; set; }
    }
}
