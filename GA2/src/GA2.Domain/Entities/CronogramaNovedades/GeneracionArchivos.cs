using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class GeneracionArchivos
    {
        public int TIPO_IDENTIFICACION_N { get; set; }
        public string TIPO_IDENTIFICACION { get; set; }
        public string NUMERO_IDENTIFICACION { get; set; }
        public string COD_MILITAR { get; set; }
        public string TOKEN { get; set; }
        public int MESES { get; set; }
        public string TIPO_AFILIADO { get; set; }
        public int FUERZA_NUMERO_1 { get; set; }
        public int FUERZA_NUMERO_2 { get; set; }
        public DateTime FECHA_REPORTE { get; set; }
        public int CODIGO_DESCUENTO { get; set; }
        public string APORTE { get; set; }
        public string I { get; set; }
        public decimal PORCENTAJE_DESCUENTO { get; set; }
        public string ACTIVACION_NOVEDAD { get; set; }
        public string NVD_NOVEDADES_ALTA_BAJA { get; set; }
        public int CODIGO_CAJA { get; set; }
        public int VALOR_DESCUENTO { get; set; }
        public int PORCENTAJE_SUELDO { get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public DateTime FECHA_FIN { get; set; }
        public int CANTIDAD_NOVEDADES { get; set; }
        public string MES_NOVEDAD { get; set; }
        public int FUERZA_CAPROVIMPO { get; set; }
        public int NUMERO_CUOTAS { get; set; }
        public string TIPO_NOVEDAD { get; set; }
        public int FUERZA_CAJAHONOR { get; set; }
        public int CODIGO_EMPRESA { get; set; }
        public DateTime FECHA_PERIODO { get; set; }
        public bool ES_ALTA { get; set; }
        public int ID_CONCEPTO_NOVEDAD { get; set; }
    }
}
