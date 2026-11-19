using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class AlternativosAplicacionPago
    {
        public int AAP_ID { get; set; }
        public string AAP_DESCRIPCION { get; set; }
        public string AAP_NOMBRE { get; set; }
        public string AAP_CODIGO { get; set; }
        public bool AAP_ESTADO { get; set; }

    }
}
