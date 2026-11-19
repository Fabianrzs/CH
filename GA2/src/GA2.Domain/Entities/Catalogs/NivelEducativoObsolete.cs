using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Nivel educativo
    /// </summary>
    [Obsolete]
    public class NivelEducativoObsolete
    {
        public Guid PRN_ID { get; set; }
        public string NVE_DESCRIPCION { get; set; }
    }
}