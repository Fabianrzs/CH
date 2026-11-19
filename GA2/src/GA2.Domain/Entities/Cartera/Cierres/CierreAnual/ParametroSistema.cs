using System;

namespace GA2.Domain.Entities.Cartera.Cierres.CierreAnual
{
    // ================================================================================
    // DTOs Y ENTIDADES PARA REESTRUCTURACIÓN
    // ================================================================================

    namespace Cartera.Entities
    {
        /// <summary>
        /// Entidad para parámetros del sistema
        /// </summary>
        public class ParametroSistema
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}

}
