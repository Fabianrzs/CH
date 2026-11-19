using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// DTO para mapear datos de PAG_PAGO_MORA
    /// </summary>
    public class PagoPagoMora
    {
        public Guid PAG_ID { get; set; }
        public Guid CRE_ID { get; set; }
        public DateTime PAG_FECHA_PAGO { get; set; }
        public DateTime PAG_FECHA_APLICACION { get; set; }
        public decimal PAG_MONTO { get; set; }
        public int PAG_DIAS_MORA_AL_PAGO { get; set; }
        public decimal PAG_CAUSACION_MORA_AL_PAGO { get; set; }
        public string PAG_ESTADO { get; set; }
        public DateTime PAG_FECHA_REGISTRO { get; set; }
    }
}
