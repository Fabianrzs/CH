using System;

namespace GA2.Domain.Entities
{
    public class AbonoHistorico
    {
        public int AH_ID { get; set; }
        public string AH_NUMERO_CREDITO { get; set; }
        public int AH_TIPO_CREDITO { get; set; }
        public decimal AH_CAPITAL { get; set; }
        public decimal AH_INTERES { get; set; }
        public decimal AH_SEGURO_VIDA { get; set; }
        public decimal AH_SEGURO_HOGAR { get; set; }
        public int AH_NUMERO_CUOTA { get; set; }
        public decimal AH_VALOR_PAGO { get; set; }
        public decimal AH_SALDO { get; set; }
        public DateTime AH_FECHA_CREACION { get; set; }
        public Guid CreadoPor { get; set; }
        public string TipoRecurso { get; set; }

    }

}