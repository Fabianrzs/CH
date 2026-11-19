using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.NewFolder
{
    public enum EnumIntegraciones
    {
        [Description("@ID_INTEGRACIONES")]
        IDINTEGRACIONES,

        [Description("@TIPOSERVICIO")]
        TIPOSERVICIO,

        [Description("@METODOHTTP")]
        METODOHTTP,

        [Description("@ENDPOINT")]
        ENDPOINT,

        [Description("@REQUEST")]
        REQUEST,

        [Description("@RESPONSE")]
        RESPONSE,

        [Description("@STATUSCODE")]
        STATUSCODE,

        [Description("@SUCCESS")]
        SUCCESS,

        [Description("@ERROR")]
        ERROR,

        [Description("@FECHAINICIO")]
        FECHAINICIO,

        [Description("@FECHAFIN")]
        FECHAFIN,

        [Description("@DURACIONMS")]
        DURACIONMS
    }
}
