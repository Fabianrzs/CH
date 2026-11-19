using System;

namespace GA2.Domain.Entities
{
    public class ResponseCreditosDesembolsadosPorPeriodo
    {
        public Guid CRE_ID { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public int TCR_ID { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
    }

}
