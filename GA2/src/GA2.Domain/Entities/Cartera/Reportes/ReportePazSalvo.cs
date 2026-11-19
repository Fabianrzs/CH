using System;

namespace GA2.Domain.Entities.Cartera.Reportes
{
    public class ReportePazSalvo
    {
        public long NUMERO_CREDITO { get; set; }
        public string IDENTIFICACION_CLIENTE { get; set; }
        public string NOMBRE_CLIENTE { get; set; }
        public decimal VALOR_DESEMBOLSO { get; set; }
        public DateTime FECHA_DESEMBOLSO { get; set; }
        public decimal SALDO_CAPITAL { get; set; }
        public decimal SALDO_CAPITAL_TRADICIONAL { get; set; }
        public decimal CAPITAL_REMANENTE { get; set; }
        public decimal INTERES_REMANENTE { get; set; }
        public int PLAZO_TOTAL { get; set; }
        public int NUMERO_CUOTAS_CANCELADAS { get; set; }
        public int NUMERO_CUOTAS_PENDIENTES { get; set; }
        public string CIUDAD_INMUEBLE { get; set; }
        public string DIRECCION_INMUEBLE { get; set; }
        public DateTime FECHA_EXPEDICION_REPORTE { get; set; }
        public string JEFE_AREA_CARTERA { get; set; }
        public string PROYECTO_ELABORO { get; set; }
        public string CIUDAD_EXPEDICION { get; set; }
        public string OBSERVACION { get; set; }
        public bool GENERAR { get; set; }
    }
}
