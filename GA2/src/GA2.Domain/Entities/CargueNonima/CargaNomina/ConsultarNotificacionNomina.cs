using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.CargueNonima.CargaNomina
{
    public class ConsultarNotificacionNomina
    {
       public Guid NRN_ID { get; set; }
       public string NRN_DESCRIPCION { get; set; }
       public int DTC_ID { get; set; }
       public DateTime NRN_FECHA_REGISTRO { get; set; }
       public string NRN_MODULO { get; set; }
       public Guid USR_USUARIO { get; set; }
       public Guid CRN_ID { get; set; } 
       public string ESTADO { get; set; }
       public int UEJ_USUARIO { get; set; }
        public int NRN_TIPO_ARCHIVO { get; set; }
        public string NRN_PERIODO { get; set; }
        public string USR_ENVIA { get; set; }
    }
}
