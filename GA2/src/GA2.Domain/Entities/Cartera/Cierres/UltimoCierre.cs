using System;

namespace GA2.Domain.Entities.Cartera.Cierres
{
    public class UltimoCierre
    {
        public int ID { get; set; }
        public string TIPO_TRANSACCION { get; set; }
        public string ANIO { get; set; }
        public string MES { get; set; }
        public DateTime FECHA_EJECUCION { get; set; }
        public DateTime? FECHA_CIERRE { get; set; }
        public string TIPO_CIERRE { get; set; }
        public int TCC_ID { get; set; }
    }
}
