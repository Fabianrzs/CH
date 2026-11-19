using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class MediosCreditoVivienda
    {
        public Guid MCV_ID { get; set; }
        public string MCV_DESCRIPCION { get; set; }
        public int MCV_ORDER { get; set; }
        public bool MCV_ESTADO {get; set; }
        public DateTime MCV_FECHA_CREACION { get; set; }
    }
}
