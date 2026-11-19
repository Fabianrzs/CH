using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class AliadosEstrategicos
    {
        public Guid ALI_ID { get; set; }
        public string ALI_RAZON_SOCIAL { get; set; }
        public DateTime ALI_FECHA_REGISTRO { get; set; }
        public bool ALI_ESTADO { get; set; }
    }
}
