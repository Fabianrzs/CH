using System;

namespace GA2.Domain.Entities.Cartera.Cierres.CierreAnual
{
    // ================================================================================
    // DTOs Y ENTIDADES PARA REESTRUCTURACIÓN
    // ================================================================================

    namespace Cartera.Entities
    {
        /// <summary>
        /// Entidad para bloqueos de crédito
        /// </summary>
        public class BloqueoCredito
    {
        public int Id { get; set; }
        public long NumeroCredito { get; set; }
        public int CodigoBloqueo { get; set; }
        public string Concepto { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public DateTime? FechaLiberacion { get; set; }
        public string CreadoPor { get; set; }
        public bool Activo { get; set; }
    }
}

}
