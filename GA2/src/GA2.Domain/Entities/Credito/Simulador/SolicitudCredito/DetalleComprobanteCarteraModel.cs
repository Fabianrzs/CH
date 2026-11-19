using System;
using System.Collections.Generic;


namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// MODEL: Detalle de Comprobante de Cartera
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class DetalleComprobanteCarteraModel
    {
        /// <summary>ID del detalle</summary>
        public Guid CPG_DET_ID { get; set; }

        /// <summary>ID del pago</summary>
        public Guid CPG_ID { get; set; }

        /// <summary>ID del comprobante (FK)</summary>
        public int CC_ID { get; set; }

        /// <summary>JSON con movimientos contables</summary>
        public string MOV_CONTABLE_SEVEN { get; set; }

        /// <summary>Respuesta de Seven</summary>
        public string RESPUESTA_SEVEN { get; set; }

        /// <summary>Fecha de ejecución</summary>
        public DateTime FECHA_EJECUCION { get; set; }

        /// <summary>Movimientos parseados del JSON</summary>
        public List<MovimientoContableSevenDto> Movimientos { get; set; }
    }
}
