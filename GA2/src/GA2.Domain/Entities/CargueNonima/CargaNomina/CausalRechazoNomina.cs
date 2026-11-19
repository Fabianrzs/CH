using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.CargueNonima.CargaNomina
{
    public class CausalRechazoNomina
    {
        public Guid CRN_ID { get; set; }
        public string CRN_DESCRIPCION { get; set; }
        public bool CRN_ESTADO { get; set; }
    }
}
