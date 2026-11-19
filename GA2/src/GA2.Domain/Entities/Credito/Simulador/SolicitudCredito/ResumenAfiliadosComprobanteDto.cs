using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// DTO: Resumen de Afiliados del Comprobante
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class ResumenAfiliadosComprobanteDto
    {
        /// <summary>ID del comprobante</summary>
        public int CC_ID { get; set; }

        /// <summary>Total de afiliados</summary>
        public int TotalAfiliados { get; set; }

        /// <summary>Lista de afiliados con sus movimientos</summary>
        public List<AfiliadoMovimientosDto> Afiliados { get; set; }
    }
}
