using System;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// DTO: Respuesta de Operación de Comprobante
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class ComprobanteCarteraOperacionDto
    {
        /// <summary>ID del comprobante procesado</summary>
        public int CC_ID { get; set; }

        /// <summary>ID del nuevo comprobante (si es reversión)</summary>
        public int? CC_ID_Nueva { get; set; }

        /// <summary>Tipo de operación realizada</summary>
        public string TipoOperacion { get; set; }

        /// <summary>Total crédito después de la operación</summary>
        public decimal TOTAL_COMPROMBANTE_CREDITO { get; set; }

        /// <summary>Total débito después de la operación</summary>
        public decimal TOTAL_COMPROMBANTE_DEBITO { get; set; }

        /// <summary>Cantidad de movimientos eliminados o invertidos</summary>
        public int CantidadMovimientos { get; set; }

        /// <summary>Cantidad de afiliados impactados</summary>
        public int CantidadAfiliados { get; set; }

        /// <summary>Estado de la operación</summary>
        public string Estado { get; set; }

        /// <summary>Fecha de la operación</summary>
        public DateTime FechaOperacion { get; set; }

        /// <summary>JSON con los movimientos después de la operación</summary>
        public string MovimientosResultado { get; set; }
    }
}
