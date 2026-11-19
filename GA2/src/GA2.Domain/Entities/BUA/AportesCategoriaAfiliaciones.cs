using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class AportesCategoriaAfiliaciones
    {
        public int APC_ID { get; set; }
        public int CLI_ID { get; set; }
        public int CTG_ID { get; set; }
        public int APC_CUOTAS_ACUMULADAS { get; set; }
        public DateTime APC_FECHA_ULTIMA_CUOTA { get; set; }
        public DateTime APC_FECHA_PRIMERA_CUOTA { get; set; }
        public DateTime APF_PERIODO_DESDE { get; set; }
        public DateTime APF_PERIODO_HASTA { get; set; }
        public int APF_RATIFICACION_CUOTAS { get; set; }
        public bool APF_LIQUIDACION_CUOTA_FONDO_SOLIDARIDAD { get; set; }
        public bool APF_REINTEGRO_APORTES { get; set; }
        public bool APF_DERECHO_LIQUIDAICION_CUOTAS { get; set; }
        public decimal APF_VALOR_DERECHO_LIQUIDACION_CUOTAS { get; set; }
    }
}
