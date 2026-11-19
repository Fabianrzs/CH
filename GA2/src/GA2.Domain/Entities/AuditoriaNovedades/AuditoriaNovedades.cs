using System;

namespace GA2.Domain.Entities
{
    public class AuditoriaNovedades
    {
        public int? TID_DESCRIPCION { get; set; }
        public string AUN_IDENTIFICACION { get; set; }
        public string? AUN_MODIFICACION { get; set; }
        public DateTime? FECHA_INICIO { get; set; }
        public DateTime? FECHA_FIN { get; set; }
        public DateTime? AUN_FECHA_MODIFICACION { get; set; }        
        public Guid? AUN_USUARIO_MODIFICACION { get; set; }
        public int ID_MODULO { get; set; }
        public string NOMBRE_MODULO { get; set; }
        public string AUN_ACCION { get; set; }
        public int UNIDAD_EJECUTORA { get; set; }
    }
}
