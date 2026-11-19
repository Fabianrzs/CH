using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Prospeccion
{
    public class InformacionProspeccion
    {
        public decimal VALOR_INMUEBLE { get; set; }
        public decimal VALOR_AVALUO { get; set; }
        public int CIUDAD_INMUEBLE { get; set; }
        public int DEPARTAMENTO_INMUBLE { get; set; }   
        public string TIPO_VIVIENDA { get; set; }
        public DateTime FECHA_PROSPECCION { get; set; }
        public decimal SALARIO_BASICO { get; set; }
        public decimal OTROS_INGRESOS { get; set; }
        public decimal INGRESO_TOTAL { get; set; }
        public decimal DESCUENTOS { get; set; }
        public decimal CAPACIDAD_PAGO { get; set; } 
        public decimal VALOR_MAXIMO_CUOTA { get; set; } 
        public decimal VALOR_MAXIMO_PRESTAR { get; set; }
        public int CUOTAS { get; set; }
    }
}
