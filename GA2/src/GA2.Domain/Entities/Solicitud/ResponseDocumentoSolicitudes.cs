using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class ResponseDocumentoSolicitudes
    {
        public Guid DCS_ID { get; set; }
        public Guid SOC_ID { get; set; }
        public string DCS_NUMERO_RADICADO { get; set; }
        public DateTime DCS_FECHA_DOCUMENTO { get; set; }
        public int DCS_NUMERO_FOLIOS { get; set; }
        public long DCS_TAMAÑO { get; set; }
        public string DCS_RUTA_IMAGEN { get; set; }
        public string DCS_ESTADO { get; set; }
        public string TDC_DESCRIPCION { get; set; }
    }
}
