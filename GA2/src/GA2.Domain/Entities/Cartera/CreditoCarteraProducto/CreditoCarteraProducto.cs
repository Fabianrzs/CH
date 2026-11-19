using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Cartera.CreditoCarteraProducto
{
    public class CreditoCarteraProducto
    {
        public InformacionInmuebleCarteraProducto InformacionInmueble { get; set; }
        public DatosAfiliadoCarteraProducto DatosAfiliadoCarteraProducto { get; set; }
        public DatosProductoCarteraProducto DatosProductoCarteraProducto { get; set; }
        public DatosSeguros DatosSeguros { get; set; }
        public BeneficiosAliviosCarteraProducto BeneficiosAliviosCarteraProducto { get; set; }
        public bool AprobadoPasoCartera { get; set; }   
        public Guid SOC_ID { get; set; }
        public Guid SMC_ID { get; set; }
    }
}
