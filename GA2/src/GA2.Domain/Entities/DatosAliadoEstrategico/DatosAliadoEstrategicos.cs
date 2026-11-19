using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.DatosAliadoEstrategico
{
    public class DatosAliadoEstrategicos
    {
        public int DAT_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public Guid DAT_NOMBRE_ALIADO { get; set; }
        public DateTime DAT_FECHA_PAGO_ESTUDIO_TITULO { get; set; }
        public DateTime DAT_FECHA_RECEPCION_ESTUDIO_TITULO { get; set; }
        public string DAT_CONCEPTO { get; set; }
        public string DAT_TIPO_PARQUEADERO { get; set; }
        public decimal DAT_PORCENTAJE_EXTRAPRIMA { get; set; }
        public string SOC_TALLA { get; set; }
        public string SOC_PESO { get; set; }
        public string DAT_DEAL_INSURANCE { get; set; }
        public string DAT_DEAL_INSURANCE_HOME { get; set; }
        public string DAT_IMC { get; set; }
        public int DAT_INSURANCE_ID { get; set; }
        public string DAT_HOME_INSURANCE_ID { get; set; }
    }
}