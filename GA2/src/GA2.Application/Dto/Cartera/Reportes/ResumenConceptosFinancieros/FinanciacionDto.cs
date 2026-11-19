using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Reportes.ResumenConceptosFinancieros
{
    public class FinanciacionDto
    {
        public decimal VALOR_INMUEBLE { get; set; }
        public decimal RECURSOS_PROPIOS { get; set; }
        public decimal TOTAL_APORTADO { get; set; }
        public decimal PORCENTAJE_TOTAL_APORTADO { get; set; }
        public string PLAZO { get; set; }
        public decimal TASA_FIJA { get; set; }
        public string MODALIDAD_PAGO { get; set; }
        public decimal OPCION_COMPRA { get; set; }
        public decimal CANON_MENSUAL { get; set; }
    }
}