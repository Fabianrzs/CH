using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class SolicitudCuenta
    {
        public Guid SSC_ID { get; set; }

        public Guid SOL_ID { get; set; }

        public decimal? SALDO_CUENTA { get; set; }

        public string? CUENTA { get; set; }

        public bool? TRASLADO_AUTORIZADO { get; set; }

        public DateTime FECHA_CREACION { get; set; }
    }
}
