using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class DatosProspeccionEndudamiento
    {

		public decimal SALARIO_ULTIMO_MES { get; set; }
        public decimal DESCUENTOS_ULTIMO_ES { get; set; }
        public decimal CAPACIDAD_PAGO { get; set; }
        public decimal CANON_MENSUAL { get; set; }
        public decimal SALARIO_NETO_ULTIMO_MES { get; set; }
        public decimal DISPONIBLE_LIBRANZA { get; set; }
        public string CONTROL_CAPACIDAD_PAGO { get; set; }
        public int RIESGO_MINIMO { get; set; }
        public decimal VALOR_SECTOR_FINANCIERO { get; set; }
        public decimal VALOR_SECTOR_REAL { get; set; }
        public decimal VALOR_OBLICACION_CANON_MENSUAL { get; set; }
        public decimal CONTROL_ENDEUDAMIENTO { get; set; }
        public string CONTROL_NIVEL_ENDEUDAMIENTO { get; set; }
    }
}
