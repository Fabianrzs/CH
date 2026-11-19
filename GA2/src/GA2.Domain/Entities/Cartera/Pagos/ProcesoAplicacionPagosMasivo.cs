using System;
using System.Collections.Generic;

namespace GA2.Domain.Entities.Cartera.Pagos
{
    public class ProcesoAplicacionPagosMasivo
    {
        public int PAP_ID { get; set; }
        public DateTime PAP_FECHA_APLICACION { get; set; }
        public DateTime PAP_FECHA_PAGO { get; set; }
        public string PAP_PERIODO { get; set; }
        public string UEJ_NOMBRE { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public int UEJ_ID { get; set; }
        public int TCR_ID { get; set; }
        public DateTime PAP_FECHA_CREACION { get; set; }
        public Guid PAP_CREADOR_POR { get; set; }
        public Guid PAP_MODIFICADO_POR { get; set; }
        public string PAP_ESTADO { get; set; }
        public string PAP_DESCRIPCION { get; set; }
        public string PAP_JSON_PAGOS { get; set; }
        public IEnumerable<PagoListaDetalle> PAP_PAGOS { get; set; } = new List<PagoListaDetalle>();
    }
}
