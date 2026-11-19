using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class ObtenerAfiliacionesRangoFiltrosDto
    {
        public int NUMERO_IDENTIFICACION { get; set; }
        public int TIPO_AFILIACION { get; set; }
        public string NUMERO_RADICADO { get; set; }
        public DateTime FECHA_RADICADO { get; set; }
        public DateTime FECHA_ELABORACION { get; set; }
    }
}
