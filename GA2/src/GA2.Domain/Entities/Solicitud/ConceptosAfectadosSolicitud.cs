using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConceptosAfectadosSolicitud
    {
        public Guid CTC_ID { get; set; }
        public string CTC_DESCRIPCION {get; set; }
        public int CTC_ORDEN {get; set;}
        public string CTC_CODIGO {get; set; }
        public DateTime CTC_FECHA_CREACION {get; set;}
        public DateTime CTC_FECHA_MODIFICACION {get; set; }
        public bool CTC_ESTADO {get; set;}
        public Guid CSA_ID_FK { get; set; }
    }
}
