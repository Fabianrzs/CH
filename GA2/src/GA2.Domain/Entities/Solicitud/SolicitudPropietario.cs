using System;

namespace GA2.Domain.Entities
{
    public class SolicitudPropietario
    {
        public Guid SOL_ID_FK { get; set; }
        public int TID_ID_FK { get; set; }
        public string PRP_NUMERO_IDENTIFICACION { get; set; }
        public string PRP_NOMBRE_RAZON_SOCIAL { get; set; }
        public Guid CREATEDO_POR { get; set; }
        public String TID_DESCRIPCION { get; set; }

        // campos adicionales
        public Guid PRP_ID { get; set; }
        public bool PRP_ESTADO { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Documento { get; set; }

        public DateTime PRP_FECHACREACION { get; set; }

    }
}
