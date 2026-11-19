using System;

namespace GA2.Domain.Entities.Cartera.Novedades
{
    public class SaldoObligacion
    {
            public Guid CRE_ID { get; set; }
			public long CRE_NRO_CREDITO { get; set; }
			public decimal SEGURO_HOGAR { get; set; }
			public decimal SEGURO_VIDA { get; set; }
			public decimal INTERES_CORRIENTE { get; set; }
			public decimal INTERES_MORA { get; set; }
			public decimal INTERES_REMANENTE { get; set; }
			public decimal CAPITAL_REMANENTE { get; set; }
            public decimal CAPITAL { get; set; }
            public decimal TOTAL_DEUDA { get; set; }
    }
}
