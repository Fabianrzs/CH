using System;

namespace GA2.Domain.Entities 
{ 
    public  class AuditoriaNovedadesGet
    {
        public string TID_DESCRIPCION { get; set; }
        public int AUN_IDENTIFICACION { get; set; }
        public string AUN_MODIFICACION { get; set; }
        public DateTime FECHA_INICIO { get; set; }
        public DateTime FECHA_FIN { get; set; }
        public DateTime AUN_FECHA_MODIFICACION { get; set; }
        public Guid AUN_USUARIO_MODIFICACION { get; set; }
        public int ID_MODULO_AUDITORIA { get; set; }
        public string nombre_modulo { get; set; }
        public string AUN_ACCION { get; set; }
        public string UNIDAD_EJECUTORA { get; set; }
        public string NOMBRE_USUARIO { get; set; }
    }
}
