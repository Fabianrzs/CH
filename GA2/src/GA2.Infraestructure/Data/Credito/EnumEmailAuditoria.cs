using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
        public enum EnumInsertarAuditoriaEmail
    {
        [Description("@EMA_ASUNTO")]
        EMA_ASUNTO,

        [Description("@EMA_DESTINATARIOS")]
        EMA_DESTINATARIOS,

        [Description("@EMA_MODULO")]
        EMA_MODULO,

        [Description("@EMA_RUTA_HTML")]
        EMA_RUTA_HTML,

        [Description("@EMA_ESTADO")]
        EMA_ESTADO,

        [Description("@EMA_FUE_EXITOSO")]
        EMA_FUE_EXITOSO,

        [Description("@EMA_MENSAJE_ERROR")]
        EMA_MENSAJE_ERROR,

        [Description("@EMA_USUARIO_CREACION")]
        EMA_USUARIO_CREACION,

        [Description("@EMA_IP_ORIGEN")]
        EMA_IP_ORIGEN
    
}
}
