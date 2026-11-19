using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace GA2.Infraestructure.Data.Verificacion
{
    public enum EnumVerificacionAbonoIntereses
    {

        [Description("@ID")]
        ID,

        [Description("@NUMERO_SOLICITUD")]
        NUMERO_SOLICITUD,

        [Description("@ANIO")]
        ANIO,

        [Description("@MES")]
        MES,        

        [Description("@TOTAL_INTERESES_ABONADOS")]
        TOTAL_INTERESES_ABONADOS,

        [Description("@USUARIO_ID")]
        USUARIO_ID,

        [Description("@USUARIO_NOMBRE")]
        USUARIO_NOMBRE,

        [Description("@FECHA_APROBACION")]
        FECHA_APROBACION,

        [Description("@FECHA_RECEPCION")]
        FECHA_RECEPCION,

        [Description("@VALOR_IPC")]
        VALOR_IPC,

        [Description("@PORCENTAJE_IPC")]
        PORCENTAJE_IPC,


        [Description("@COMPROBANTE_SOLUCION_VIVIENDA_APORTES")]
        COMPROBANTE_SOLUCION_VIVIENDA_APORTES,

        [Description("@COMPROBANTE_SOLUCION_VIVIENDA_CESANTIAS")]
        COMPROBANTE_SOLUCION_VIVIENDA_CESANTIAS,

        [Description("@COMPROBANTE_SOLUCION_ADMINISTRACION_CESANTIAS")]
        COMPROBANTE_SOLUCION_ADMINISTRACION_CESANTIAS,


        [Description("@COMPROBANTE_AVAC")]
        COMPROBANTE_AVAC,

        [Description("@COMPROBANTE_AVAV")]
        COMPROBANTE_AVAV,

        [Description("@COMPROBANTE_AVC")]
        COMPROBANTE_AVC,


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
