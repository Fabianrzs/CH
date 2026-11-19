using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class RequestActividadTramite
    {
        public Guid ACT_ID { get; set; }
        public Guid TRS_ID { get; set; }
        public string ACT_OBSERVACION { get; set; }
        public Guid ACT_MODIFICADO_POR { get; set; }

    }


    public class InsertarNotificacionFuncionarioExterno
    {
        public string Modulo { get; set; }
        public string Mensaje { get; set; }
        public string Perfil { get; set; }
        public Guid? UsuarioEnvia { get; set; } = Guid.Empty;

    }



    public class ResponseActividadTramite
    {
        public Guid ACT_ID { get; set; }
        public Guid TRS_ID { get; set; }
        public int AFL_ID { get; set; }
        public Guid ID_USER { get; set; }
        public DateTime ACT_FECHA_INICIO { get; set; }
        public DateTime ACT_FECHA_FINALIZA { get; set; }
        public string ACT_OBSERVACION { get; set; }
        public int ACT_ESTADO { get; set; }
        public Guid ACT_CREADO_POR { get; set; }
        public DateTime ACT_FECHA_CREACION { get; set; }
        public Guid ACT_APROVADO_POR { get; set; }
        public DateTime ACT_FECHA_APROVACION { get; set; }
        public Guid ACT_MODIFICADO_POR { get; set; }
        public DateTime ACT_FECHA_MODIFICACION { get; set; }

    }
}
