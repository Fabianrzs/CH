using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class TareaSolicitud
    {
        public int AST_ID { get; set; }
	    public string AST_MENSAJE { get; set; }
        public string AST_LINK { get; set; }
        public Guid AST_ESTADO { get; set; }
        public Guid USR_ID { get; set; }
        public Guid? FLJ_ID { get; set; }
        public DateTime AST_FECHA_REGISTRO { get; set; }
        public DateTime AST_FECHA_FINALIZACION { get; set; }
        public string Radicado { get; set; }
        public string Modulo { get; set; }
        public string IdentificacionAfiliado { get; set; }
        public string? USR_ENVIA { get; set; }
        public Guid? SOL_ID { get; set; }
        public string? Estado_Tarea { get; set; }
        public string? TipoSolicitud { get; set; }
        public string? TipoModalidad { get; set; }
        public string? TipoSubModalidad { get; set; }
        public Guid? IdTipoSolicitud { get; set; }
        public Guid? IdTipoModalidad { get; set; }
        public Guid? IdTipoSubModalidad { get; set; }
        public string NOMBRE_TAREA { get; set; }

    }
}
