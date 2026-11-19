using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionDerechoParameters
    {

        [Description("@ID")]
        ID,

        [Description("@NUMERO_SOLICITUD")]
        NUMERO_SOLICITUD,

        [Description("@ANIO")]
        ANIO,

        [Description("@MES")]
        MES,

        [Description("@FECHA_OPERACION")]
        FECHA_OPERACION,

        [Description("@FECHA_APROBACION")]
        FECHA_APROBACION,

        [Description("@TOTAL_CUENTAS_VERIFICADAS")]
        TOTAL_CUENTAS_VERIFICADAS,

        [Description("@TOTAL_CAMBIOS_ESTADO")]
        TOTAL_CAMBIOS_ESTADO,

        [Description("@USUARIO_ID")]
        USUARIO_ID,

        [Description("@USUARIO_NOMBRE")]
        USUARIO_NOMBRE,

        [Description("@APROBADO")]
        APROBADO,

        [Description("@NUMERO_RADICADO")]
        NUMERO_RADICADO,


        [Description("@ESTADO")]
        ESTADO,

        [Description("@NOMBRE_ESTADO")]
        NOMBRE_ESTADO,

        [Description("@SLT_ID")]
        SLT_ID,

        [Description("@SLT_ID_APROBAR")]
        SLT_ID_APROBAR,

    }
}
