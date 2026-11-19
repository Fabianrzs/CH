using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class SolicitudAdjudicacion
    {
        public Guid SIA_ID { get; set; }
        public Guid SOL_ID_FK { get; set; }
        public string SIA_TIPO_VIVIENDA { get; set; }
        public string SIA_DIRECCION { get; set; }
        public string SIA_MATRICULA { get; set; }
        public string SIA_VALOR_SUBSIDIADO { get; set; }
        public string SIA_CIUDAD { get; set; }
    }
}
