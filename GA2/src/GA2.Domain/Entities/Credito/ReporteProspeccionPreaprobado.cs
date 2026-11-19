using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class ReporteProspeccionPreaprobado
    {
        public Guid RTU_ID { get; set; }

        public DateTime RTU_FECHA_CONSULTA { get; set; }

        public string RTU_NUMERO_IDENTIFICACION { get; set; }

        public string RTU_NOMBRE { get; set; }

        public string RTU_DEPARTAMENTO { get; set; }

        public string RTU_CIUDAD { get; set; }

        public string RTU_TIPO_CREDITO { get; set; }

        public decimal RTU_VALOR_INMUEBLE { get; set; }

        public int RTU_ESTADO { get; set; }

        public string RTU_MOTIVO { get; set; }

        public Guid? RTU_CREADO_POR { get; set; }
    }
}
