using System;

namespace GA2.Domain.Entities.Reportes
{
    public class ReporteAltasBajas
    {
        public string IDENTIFICACION { get; set; }
        public decimal VALOR_CUOTA { get; set; }
        public string NUMERO_CREDITO { get; set; }
        public string TIPO { get; set; }
        public DateTime FECHA_DOCUMENTO_SOPORTE { get; set; }
        public string MODALIDAD { get; set; }
        public string CODIGO_DESCUENTO { get; set; }
        public string FECHA_REPORTE { get; set; }
        public decimal MONTO_DESEMBOLSADO { get; set; }
        public string CODIGO_MILITAR { get; set; }
        public string NUMERO_CLIENTE_CYGNUS { get; set; }
        public string VACIO { get; set; }
        public string FECHA_INICIO_CUOTA { get; set; }
        public string FECHA_FIN_CUOTA { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public int NUMERO_CUOTAS { get; set; }
        public string GRADO { get; set; }
        public string CONCEPTO { get; set; }
    }
}
