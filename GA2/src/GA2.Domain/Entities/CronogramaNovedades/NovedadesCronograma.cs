using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class NovedadesCronograma
    {
        public int NVD_ID { get; set; }
	    public int CNV_ID { get; set; }
        public string NVD_CODIGO_ALTA { get; set; }
        public string NVD_CODIGO_BAJA { get; set; }
        public int UEJ_ID { get; set; }
    }
}
