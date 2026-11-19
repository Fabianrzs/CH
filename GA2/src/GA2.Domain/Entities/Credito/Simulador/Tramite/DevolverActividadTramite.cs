using System;

namespace GA2.Domain.Entities
{
    public class DevolverActividadTramite
    {
        public Guid TRS_ID { get; set; }
        public int ACT_ID { get; set; }
        public int ACT_ID_RETORNO { get; set; }
        public string ACT_OBSERVACION { get; set; }
        public Guid ACT_MODIFICADO_POR { get; set; }
    }
}
