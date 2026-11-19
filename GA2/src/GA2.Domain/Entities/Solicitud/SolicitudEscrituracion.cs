using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class SolicitudEscrituracion
    {
        public Guid SIE_ID { get; set; }
        public Guid SOL_ID_FK { get; set; }
        public string SIE_NOTARIA { get; set; }
        public string SIE_NUMERO_ESCRITURACION { get; set; }
        public string SIE_FECHA_ESCRITURACION { get; set; }
    }
}
