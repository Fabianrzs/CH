using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ConsultarNumeroFolioSolicitud
    {

        public int SOL_NUMERO_SOLICITUD { get; set; }
        public int NUMERO_FOLIO { get; set; }

        public string SOL_RECHAZADA { get; set; }
        public DateTime? FECHA_FIRMA_ESCRITURA { get; set; }
    }
}
