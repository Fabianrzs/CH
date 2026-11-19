using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Catalogs.ComprobantesCartera
{
    public class TipoBeneficiario
    {
        public int TBC_ID { get;set; }
        public string TBC_TIPO_BENEFICIARIO { get; set; }
        public string TBC_TABLA_BENEFICIARIO { get; set; }
        public DateTime TBC_FECHA_CREACION { get; set; }
        public Guid TBC_CREADO_POR { get; set; }
        public DateTime TBC_FECHA_MODIFICACION { get; set; }
        public Guid TBC_MODIFICADO_POR { get; set; }
        public bool TBC_ACTIVO { get; set; }
    }
}
