using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ConceptosCongeladosProspeccion
    {
        public Guid CON_ID { get; set; }
        public string CON_DESCRIPCION { get; set; }
        public double CON_VALOR { get; set; }
        public DateTime CON_FECHA_REGISTRO { get; set; }
        public Guid SLS_ID { get; set; } 
        public string CON_NUMERO_DOCUMENTO { get; set; }
        public int CON_TIPO_IDENTIFICACION { get; set; }
        public string CON_TIPO_CREDITO { get; set; }
        public double CON_VALOR_CALCULADO { get; set; }
        public double CON_PORCENTAJE { get; set; }
        public string CON_VALOR_FORMAT { get; set; }
        public string CON_PORCENTAJE_FORMAT { get; set; }
        public string CON_VALOR_CALCULADO_FORMAT { get; set; }
    }
}
