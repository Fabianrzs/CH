using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.VerificacionDerechos
{
    public class NotificacionTarea
    {

        public Guid USR_ID { get; set; }
        public string USR_IDENTIFICACION { get; set; }

        public string USR_NOMBRE { get; set; }

        public string USR_APELLIDO { get; set; }

        public string AST_LINK { get; set; }

        public Guid Usuario { get; set; }
        public string Notificacion { get; set; }
        public string NumRadicado { get; set; }
        public string Modulo { get; set; }
        public string NUM_CEDULA { get; set; }
        public Guid TareaSiguiente { get; set; }
        public Guid UsuarioEnvia { get; set; }



    }
}
