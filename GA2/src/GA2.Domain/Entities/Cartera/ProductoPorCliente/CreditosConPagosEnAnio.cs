using System;

namespace GA2.Domain.Entities
{
    public class CreditosConPagosEnAnio
    {
        public Guid CRE_ID { get; set; }
        public long CRE_NRO_CREDITO { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public int TCR_ID { get; set; }
    }
}
