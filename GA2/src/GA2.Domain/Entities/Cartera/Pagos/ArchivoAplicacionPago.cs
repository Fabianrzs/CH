using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ArchivoAplicacionPagoTXT
    {
        public int CODIGO1 { get; set; }
        public int CODIGO2 { get; set; }
        public string AREA { get; set; }
        public string GRADO { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string NRO_IDENTIFICACION { get; set; }
        public string NOMBRES { get; set; }
        public int CONCEPTO { get; set; }
        public decimal VALOR_CUOTA { get; set; }
        public decimal SALDO { get; set; }
        public string DESCRIPCION { get; set; }
    }

    public class ArchivoAplicacionPagoCSV
    {
        public string FUERZA_NOMBRE { get; set; }
        public string TIPO_NOMINA { get; set; }
        public string COD_CONCEPTO { get; set; }
        public string CONCEPTO { get; set; }
        public string FUNCIONARIO { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }        
        public string NRO_IDENTIFICACION { get; set; }
        public string CODIGO_MILITAR { get; set; }
        public decimal VALOR_CUOTA { get; set; }
        public DateTime FECHA_DESDE { get; set; }
        public DateTime FECHA_HASTA { get; set; }
    }

}
