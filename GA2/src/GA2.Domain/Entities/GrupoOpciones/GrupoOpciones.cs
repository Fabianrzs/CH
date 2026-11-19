using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Domain.Entities
{
    public class GrupoOpciones
    {
        public int ID { get; set; }
        public string CODIGO { get; set; }
        public string NOMBRE { get; set; }
        public string DESCRIPCION { get; set; }
        public string ESTADO { get; set; }
        public int GRP_ID { get; set; }
        public Guid USR_CREADOR { get; set; }  
        public DateTime GRP_FECHA_CREACION { get; set; }
        public Guid USR_MODIFICADOR { get; set; }
        public DateTime GRP_FECHA_MODIFICACION { get; set; }

    }
}
