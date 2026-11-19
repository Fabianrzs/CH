using System;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// DTO: Solicitud para Crear Reversión
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class CrearReversionComprobanteDto
    {
        /// <summary>ID del comprobante original a revertir</summary>
        public int CC_ID { get; set; }

        /// <summary>Usuario que crea la reversión</summary>
        public Guid CreadoPor { get; set; }

        /// <summary>Razón de la reversión (opcional)</summary>
        public string Razon { get; set; }
    }
}
