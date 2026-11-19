using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GA2.Application.Dto.Credito
{
    public class DatosAdicionalesSimulacionDto
    {
        public Guid id { get; set; }
        public Guid Mcv_id {get; set;}
        public int numFolios { get; set;}
        public int mr_id { get; set;}
    }
}
