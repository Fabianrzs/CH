using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.Novedades
{
    public class NovedadCartera
    {
        public Guid ID_NOVEDAD { get; set; }
        public Guid CRE_ID { get; set; }
        public int TNV_ID { get; set; }
        public string TIPO_NOVEDAD { get; set; }
        public DateTime FECHA_NOVEDAD { get; set; }
        public DateTime FECHA_FINALIZACION { get; set; }
        public int CAN_ID { get; set; }
        public string TIPO_CAUSAL { get; set; }
        public int ENC_ID { get; set; }
        public string ENC_CODIGO { get; set; }
        public string ESTADO_NOVEDAD { get; set; }

        public Guid NVC_CREADO_POR { get; set; }
        public DateTime NVC_FECHA_CREACION { get; set; }
        public Guid? NVC_MODIFICADO_POR { get; set; }
        public DateTime? NVC_FECHA_MODIFICACION { get; set; }


        // Valores calculados
        public decimal NVC_CAPITAL { get; set; }
        public decimal NVC_SEGURO_VIDA { get; set; }
        public decimal NVC_SEGURO_HOGAR { get; set; }
        public decimal NVC_INTERESES_CORRIENTES { get; set; }
        public decimal NVC_INTERESES_MORA { get; set; }
        public decimal NVC_CAPITAL_REMANENTE { get; set; }
        public decimal NVC_INTERES_REMANENTE { get; set; }
        public decimal NVC_TOTAL_DEUDA { get; set; }
        public decimal VALOR_PAGADO { get; set; }
        public decimal VALOR_RESTANTE { get; set; }

        public DateTime? NVC_FECHA_PROXIMO_PAGO{ get; set; }

        public decimal NVC_VALOR_NUEVA_TASA { get; set; }

    }
}
