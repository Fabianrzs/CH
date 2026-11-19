using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.CargueNonima.CargaNomina
{
    public class InformacionAfiliadoCargueNomina
    {
            public Guid ICN_ID { get; set; }
            public string ICN_AFILIADO { get; set; }
            public string ICN_NUMERO_DOCUMENTO { get; set; }
            public string ICN_UNIDAD_EJECUTORA { get; set; }
            public string ICN_TIPO_CUENTA { get; set; }
            public string ICN_ESTADO_CUENTA { get; set; }
            public decimal ICN_AHORROS_OBLIGATORIOS { get; set; }
            public decimal ICN_AHORROS_VOLUNTARIOS { get; set; }
            public decimal ICN_CESANTIAS { get; set; }
            public decimal ICN_AHORROS_RETROACTIVOS { get; set; }
            public decimal ICN_CESANTIAS_RETROACTIVOS { get; set; }
            public decimal ICN_INTERESES_APORTES { get; set; }
            public decimal ICN_INTERESES_CESANTIAS { get; set; }
            public decimal ICN_CESANTIAS_DOCEAVAS { get; set; }
            public decimal ICN_TOTAL_CONCEPTO { get; set; }
            public DateTime ICN_FECHA_REGISTRO { get; set; }

        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }


    }
}
