using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reportes
{
    public  class ReporteJefeDto
    {
        public int TIPO_IDENTIFICACION { get; set; }
        public String IDENTIFICACION { get; set; }
        public String SEGUNDO_APELLIDO { get; set; }
        public String PRIMER_APELLIDO { get; set; }
        public String PRIMER_NOMBRE { get; set; }
        public String SEGUNDO_NOMBRE { get; set; }
        public String NUMERO_RADICADO { get; set; }

        public String PUNTO_RADICACION { get; set; }

        public String MODELO_LEASING { get; set; }

        public String UNIDAD_EJECUTORA { get; set; }

        public String CATEGORIA { get; set; }

        public String PREAPROBADO { get; set; }

        public String MODALIDADES_NEGACION { get; set; }

        public String ID_APROBADOCOMITE { get; set; }

        public String MODALIDADES_NEGACION_COMITE { get; set; }

        public String PROCESO { get; set; }

        public String PASO { get; set; }

        public String NOMBRE_PASO { get; set; }

        public String ESTADO { get; set; }

        public String RESPONSABLE { get; set; }

        public DateTime FECHA_INICIO_PROCESO { get; set; }

        public DateTime FECHA_REAL_FINALIZACION { get; set; }

        public String TIEMPO_REAL_EJECUCION { get; set; }

        public byte[] FILE { get; set; }
    }
}
