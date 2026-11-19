using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ResultadoIntegracion
    {
      //  public int PIN_ID { get; set; }
        public string TID_ID { get; set; }
        public string PIN_NRO_IDENTIFICACION { get; set; }
        public Guid PIN_PARAM_ID { get; set; }
        public DateTime PIN_FECHA_EJECUCION { get; set; }
        public string PIN_RESULTADO_INTEGRACION { get; set; }
        public Guid USR_ID { get; set; }

    }
}
