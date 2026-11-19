using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class AportesCategoriaAfiliacionesDto
    {
        public int clI_ID { get; set; }
        public int ctG_ID { get; set; }
        public int apC_CUOTAS_ACUMULADAS { get; set; }
        public DateTime apC_FECHA_ULTIMA_COUTA { get; set; }
        public DateTime apC_FECHA_PRIMERA_CUOTA { get; set; }
        public string ctG_DESCRIPCION { get; set; }
        public DateTime periodoAportesDesde { get; set; }
        public DateTime periodoAportesHasta { get; set; }
        public int ratificacionCuotas { get; set; }
        public bool liquidacionCuotaFondoSolidaridad { get; set; }
        public bool reintegroAportes { get; set; }
        public bool derechoLiquidacionCuotas { get; set; }
        public decimal valorDerechoLiquidacionCuotas { get; set; }
    }
}
