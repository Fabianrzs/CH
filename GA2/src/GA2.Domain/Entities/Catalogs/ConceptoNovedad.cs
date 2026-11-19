using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class ConceptoNovedad
    {
        public int CNV_ID { get; set; }
        public int CNC_ID { get; set; }
        public string CNV_DESCRIPCION { get; set; }
        public bool CNV_ALTA { get; set; }
    }
}
