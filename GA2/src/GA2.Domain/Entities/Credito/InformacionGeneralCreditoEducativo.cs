using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Credito
{
    public class InformacionGeneralCreditoEducativo
    {
        public InformacionSolicitudCreditoEducativo InformacionSolicitudCredito { get; set; }
        public DetalleProductoCreditoEducativo DetalleProducto { get; set; }
        public BeneficiarioEntidadEducativa EntidadEducativa { get; set; }

    }
}
