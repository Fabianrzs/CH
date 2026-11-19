using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Reportes
{
    public class DocumentosAdicionalesCargados
    {
        public Guid DSA_ID { get; set; }
        public string DSA_DOCUMENTO  { get; set; }
        public string DSA_RUTA { get; set; }
        public string DSA_CARPETA{ get; set; }
        public DateTime DSA_FECHA_CARGUE{ get; set; }
        public bool @DSA_CARGADO_DODO { get; set; }
        public string DSA_MODULO { get; set; }
        public string DSA_NUMERO_DOCUMENTO { get; set; }
    }
}
