using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities.Solicitud
{
    public  class UsuarioIngresoSolicitud
    {
        public string IDENTIFICACION_USUARIO { get; set; }
        public string NOMBRES_USUARIO { get; set; }
        public string APELLIDOS_USUARIO { get; set; }   
        public Guid ID_USR { get; set; }   
    }
}
