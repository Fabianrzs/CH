using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Catalogs
{
    public class LineaCredito
    {
        public int CLC_ID { get; set; }
        public string CLC_DESCRIPCION { get; set; }
        public int TCR_ID { get; set; }
        public int? TPA_ID {  get; set; }
        public int? CLC_CUOTA_MINIMA { get; set; }
        public int? CLC_CUOTA_MAXIMA { get; set; }
        public DateTime CLC_FECHA_CREACION { get; set; }
    }
}
