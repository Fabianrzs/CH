using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.ValidacionTelAfiliado
{
    public class ValidacionTelAfiliados
    {
        public int VAL_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public decimal VAL_CONF_VAL_INM { get; set; }
        public int VAL_ANIOS_FUERZA { get; set; }
        public string VAL_LUGAR_TRABAJO { get; set; }
        public string VAL_CARGO_ACTUAL { get; set; }
        public decimal VAL_VALOR_ARRIENDO { get; set; }
        public decimal VAL_VALOR_ACTIVOS { get; set; }
        public decimal VAL_VALOR_PASIVOS { get; set; }
        public decimal VAL_MONTOS_RECURSOS_PROPIOS { get; set; }
        public string VAL_OBSERVACIONES { get; set; }
    }
}