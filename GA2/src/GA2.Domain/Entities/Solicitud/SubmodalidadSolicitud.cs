using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public class SubmodalidadSolicitud
    {
        public Guid TPS_SUB_ID { get; set; }
        public Guid TIM_ID { get; set; }
        public string TPS_SUB_DESCRIPCION { get; set; }
        public bool TPS_SUB_ACTIVO { get; set; }
        public int TPS_SUB_ORDEN { get; set; }
        public Guid TPS_SUB_CREATEDOPOR { get; set; }
        public DateTime TPS_SUB_FECHACREACION { get; set; }
        public Guid TPS_SUB_MODIFICADOPOR { get; set; }
        public DateTime TPS_SUB_FECHAMODIFICACION { get; set; }
        public string SOLICITUD { get; set; }
        public string TIPO_SUBMODALIDAD { get; set; }
        public string TRAMITE_PAGO { get; set; }
        public string MODALIDAD_PAGO { get; set; }
        public string TIPO_VIVIENDA { get; set; }
        public Guid TPS_PRIMER_PAGO { get; set; }
    }
}
