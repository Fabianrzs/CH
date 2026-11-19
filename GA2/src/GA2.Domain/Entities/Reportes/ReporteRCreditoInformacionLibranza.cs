using System;

namespace GA2.Domain.Entities.Reportes
{
    public class ReporteRCreditoInformacionLibranza
    {
        public DateTime FECHA_EVALUACION { get; set; }
        public string NIT_EMPRESA { get; set; }
        public string NOMBRE_EMPRESA { get; set; }
        public string CARACTER_EMPRESA { get; set; }
        public string SECTOR_EMPRESA { get; set; }
        public string ESTADO_PAGADURIA { get; set; }
        public int PLAZO_MAXIMO { get; set; }
        public int NUMERO_OBLIGACIONES { get; set; }
        public int NUMERO_DEUDORES { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
        public int NUMERO_OBLIGACIONES_MORA { get; set; }
        public int NUMERO_DEUDORES_MORA { get; set; }
        public decimal SALDO_CAPITAL_MORA { get; set; }
    }
}
