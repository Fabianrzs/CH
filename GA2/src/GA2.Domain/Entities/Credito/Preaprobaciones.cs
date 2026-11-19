using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class Preaprobaciones
    {
       
        public string SMC_NUMERO_DOCUMENTO { get; set; }
        public double SMC_VALOR_PRESTAMO { get; set; }
        public DateTime SMC_FECHA_SIMULACION { get; set; }
        public int SMC_NUMERO_PREAPROBADO { get; set; }
        public DateTime SMC_FECHA_APROBACION { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public string SLS_ESTADO { get; set; }
        public string CLI_PRIMER_NOMBRE { get; set; }
        public string CLI_SEGUNDO_NOMBRE { get; set; }
        public string CLI_PRIMER_APELLIDO { get; set; }
        public string CLI_SEGUNDO_APELLIDO { get; set; }
      

    }


}
