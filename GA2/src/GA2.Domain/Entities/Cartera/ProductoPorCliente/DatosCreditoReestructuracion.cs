using System;

namespace GA2.Domain.Entities
{
    public class DatosCreditoReestructuracion
    {
        public long CRE_NUMERO_CREDITO { get; set; }
        public int CRE_TIPO_CREDITO { get; set; }
        public string CRE_ESTADO { get; set; }
        public decimal CRE_TASA { get; set; }
        public decimal CRE_TASA_MORA { get; set; }
        public int CRE_PLAZO { get; set; }
        public decimal CRE_VALOR_CUOTA { get; set; }
        public decimal CRE_SALDO_CAPITAL { get; set; }
        public DateTime CRE_FECHA_DESEMBOLSO { get; set; }
        public int TotalCuotas { get; set; }
        public int CuotasPagadas { get; set; }
        public int DiasMoraActual { get; set; }
    }
}
