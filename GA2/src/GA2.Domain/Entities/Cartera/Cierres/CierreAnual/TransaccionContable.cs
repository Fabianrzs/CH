using System;

namespace GA2.Domain.Entities.Cartera.Cierres.CierreAnual
{
    // ================================================================================
    // DTOs Y ENTIDADES PARA REESTRUCTURACIÓN
    // ================================================================================

    namespace Cartera.Entities
    {
        /// <summary>
        /// Entidad para registrar transacciones contables
        /// </summary>
        public class TransaccionContable
        {
            public int Id { get; set; }
            public long NumeroCredito { get; set; }
            public string TipoTransaccion { get; set; } // PAGO_REESTRUCTURACION, CARGO_REESTRUCTURACION
            public decimal Monto { get; set; }
            public DateTime Fecha { get; set; }
            public string Descripcion { get; set; }
            public string CreadoPor { get; set; }
            public DateTime FechaCreacion { get; set; }
        }
    }
}

