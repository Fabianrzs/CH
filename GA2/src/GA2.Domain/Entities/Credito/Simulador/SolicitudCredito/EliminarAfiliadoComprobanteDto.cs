using System;
using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    /// ═════════════════════════════════════════════════════════════════════════════════
    /// DTO: Solicitud para Eliminar Afiliado
    /// ═════════════════════════════════════════════════════════════════════════════════
    public class EliminarAfiliadoComprobanteDto
    {
        /// <summary>ID del comprobante</summary>
        public int CC_ID { get; set; }

        /// <summary>Referencia del afiliado/crédito a eliminar</summary>
        public List<string> Dmc_refe { get; set; }

        /// <summary>Usuario que realiza la acción</summary>
        public Guid? ModificadoPor { get; set; }
    }
}
