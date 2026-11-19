using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito.TransUnion
{
    public class UpdateSlsTu
    {
        public Guid SLS_ID { get; set; }
        public DateTime SLS_FECHA_CON_TU { get; set; }
        public string SLS_XML { get; set; }
    }
}
