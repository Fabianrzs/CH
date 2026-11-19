using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Catalogs.ComprobantesCartera
{
    public class TipoConcepto
    {
        public int TCC_ID { get; set; }
        public string TCC_TIPO_CONCEPTO { get; set; }
        public DateTime TCC_FECHA_CREACION {  get; set; }
        public Guid TCC_CREADO_POR { get; set; }
        public DateTime TCC_FECHA_MODIFICACION { get; set; }
        public Guid TCC_MODIFICADO_POR { get; set; }
        public bool TCC_ACTIVO { get; set; }
    }
}
