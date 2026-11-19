using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class TareasSolicitudDto
    {
        public Guid IdUsuario { get; set; }
        public Guid IdUsuarioAAsignar { get; set; }
        public string? USR_ENVIA { get; set; }
        public Guid? SOL_ID { get; set; }
        public string? Estado_Tarea { get; set; }
        public DateTime AST_FECHA_REGISTRO { get; set; }
        public Guid? IdFlujo { get; set; }
        public string IdentificaciónCliente { get; set; }
        public string IdentificadorObjetoDesbloqueo { get; set; }
        public string Asunto { get; set; }
        public string Tema { get; set; }
        public string Descripcion { get; set; }
        public string Link { get; set; }
        public string Estado { get; set; }
        public string Radicado { get; set; }
        public string Modulo { get; set; }
        public string IdentificacionAfiliado { get;set; }
        public string? TipoSolicitud { get;set; }
        public string? TipoModalidad { get;set; }
        public string? TipoSubModalidad { get;set; }
        public Guid? IdTipoSolicitud { get; set; }
        public Guid? IdTipoModalidad { get; set; }
        public Guid? IdTipoSubModalidad { get; set; }

        // inconsistencia
        public int? ID { get; set; }
        public Guid? SLT_TAREA { get; set; }
        public Guid? IdSolicitud { get; set; }
        public string? NOMBRE_REGLA { get; set; }
        public Guid? CREADORPOR { get; set; }
        public DateTime? FECHACREACION { get; set; }
        public string? TRA_NOMBRE { get; set; }

     


    }
}
    