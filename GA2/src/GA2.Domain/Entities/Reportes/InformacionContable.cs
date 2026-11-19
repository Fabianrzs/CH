using System;

namespace GA2.Domain.Entities.Reportes
{
    public class InformacionContable
    {
        public DateTime FECHA_MOVIMIENTO { get; set; }
        public int? COMPROBANTE { get; set; }
        public long TIPO_COMPROBANTE { get; set; }
        public string DETALLE { get; set; }
        public string MOVIMIENTO { get; set; }
        public decimal VALOR { get; set; }
        public decimal SALDO { get; set; }
        public string CONCEPTO { get; set; }
        public string TERCERO { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string NUMERO_IDENTIFICACION { get; set; }
        public string NOMBRE_TERCERO { get; set; }
        public string NUMERO_CUENTA_CONTABLE { get; set; }
        public string NOMBRE_CUENTA_CONTABLE { get; set; }
    }
}