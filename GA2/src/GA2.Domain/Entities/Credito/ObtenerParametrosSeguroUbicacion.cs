using System;

namespace GA2.Domain.Entities
{
    public class ObtenerParametrosSeguroUbicacion
    {
        public int SEU_ID { get; set; }
        public string SEU_TODO_RIESGO { get; set; }
        public string DPD_ID { get; set; }
        public int DPC_ID { get; set; }
        public DateTime SEU_FECHA_MODIFICACION { get; set; }
        public int SEU_MODIFICADO_POR { get; set; }
        public int SEU_ESTADO { get; set; }
        public string DPC_NOMBRE { get; set; }
        public string DPD_NOMBRE { get; set; }
    }
}
