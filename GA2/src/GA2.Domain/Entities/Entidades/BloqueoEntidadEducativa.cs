using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Entidad educativa
    /// </summary>
    public class BloqueoEntidadEducativa
    {
        public Guid ENE_BLOQUEO { get; set; }
        public Guid ENE_ID { get; set; }
        public int ENE_TIPO_OPERACION { get; set; }
        public int ENE_CAUSAL_BLOQUEO { get; set; }
        public DateTime ENE_FECHA_BLOQUEO { get; set; }
        public Guid ENE_FUNCIONARIO_BLOQUEO { get; set; }
        public int TIPO_OPERACION_ID { get; set; }
        public string NOMBRE_OPEREACION { get; set; }
        public int CAUSAL_OPERACION_ID { get; set; }
        public string NOMBRE_CAUSAL { get; set; }
        public string USR_USERNAME { get; set; }
        public string ENE_OBSERVACIONES { get; set; }
    }
}