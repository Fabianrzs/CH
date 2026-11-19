using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Tabla de CausalesRechazo
    /// <summary>
    /// Propiedades CausalesRechazo 
    /// </summary>
    public class CausalesRechazo
    {
        public int COS_CAU_ID { get; set; }
        public string DESCRIPCION { get; set; }
        public int ESTADO { get; set; }
        public Guid CREADO_POR { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public Guid MODIFICADO_POR { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
    }
}
