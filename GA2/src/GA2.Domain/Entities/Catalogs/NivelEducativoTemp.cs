using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Nivel educativo
    /// </summary>
    public class NivelEducativoTemp
    {
        public int CVL_ID { get; set; } 
        public int CAT_ID { get; set; }
        public string CVL_CODIGO { get; set; }
        public string CVL_DESCRIPCION { get; set; }
        public bool CVL_ACTIVO { get; set; }
    }
}