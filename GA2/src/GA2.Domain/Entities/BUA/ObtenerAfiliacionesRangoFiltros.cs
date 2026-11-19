using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class ObtenerAfiliacionesRangoFiltros
    {
        public int CLD_ID { get; set; }
        public string AFL_NUMERO_RADICADO { get; set; }
        public string AFL_FECHA_RADICADO { get; set; }
        public string AFL_FECHA_ELABORACION { get; set; }
        public int TPF_ID { get; set; }
    }
}
