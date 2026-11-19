using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.ServiciosDinamicosEnt
{
    public class ServiciosDinamicos
    {
        public Guid SRD_ID { get; set; }
        public string SRD_DESCRIPCION { get; set; }
        public bool SRD_ESTADO { get; set; }
    }
}
