using System;

namespace GA2.Domain.Entities
{
    public class ActualizarSolicitud
    {
        public Guid SOL_ID { get; set; }
        public decimal SOL_VALOR_A_RETIRAR { get; set; }
        public bool? Posee_Avac { get; set; } 
        public int NUMERO_FOLIO { get; set; }

        public DateTime? FECHA_FIRMA_ESCRITURA { get; set; }
    }
}
