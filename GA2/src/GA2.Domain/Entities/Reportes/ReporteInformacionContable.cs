using System;

namespace GA2.Domain.Entities.Reportes
{
    public class ReporteInformacionContable
    {
        public DateTime FECHA_MOVIMIENTO { get; set; }
        public decimal VALOR { get; set; }
        public int COMPROBANTE { get; set; }
        public string MOVIMIENTO { get; set; }
        public string TIPO_COMPROBANTE { get; set; }
        public string NUMERO_CUENTA_CONTABLE { get; set; }
        public string NOMBRE_CUENTA_CONTABLE { get; set; }
    }
}
