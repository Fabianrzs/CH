using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;


//namespace GA2.Infraestructure.Data.Credito.Simulador.DatosPersonales


namespace GA2.Infraestructure.Data
{
    public enum EnumSolicitudDatosPersonalesParams
    {

        [Description("@IND")]
        IND,

        [Description("@SDP_ID")]
        SDP_ID,

        [Description("@SDP_NUMERO_DOCUMENTO")]
        SDP_NUMERO_DOCUMENTO,

        [Description("@CTG_ID")]
        CTG_ID,

        [Description("@GRD_ID")]
        GRD_ID,

        [Description("@SDP_NOMBRES_APELLIDOS")]
        SDP_NOMBRES_APELLIDOS,

        [Description("@SDP_EDAD")]
        SDP_EDAD,

        [Description("@DPD_ID")]
        DPD_ID,

        [Description("@DPC_ID")]
        DPC_ID,

        [Description("@SDP_DIRECCION")]
        SDP_DIRECCION,

        [Description("@SDP_TELEFONO_FIJO")]
        SDP_TELEFONO_FIJO,

        [Description("@SDP_TELEFONO_CELULAR")]
        SDP_TELEFONO_CELULAR,

        [Description("@SDP_E_MAIL")]
        SDP_E_MAIL,

        [Description("@SDP_CUOTAS")]
        SDP_CUOTAS,

        [Description("@SDP_REGIMEN")]
        SDP_REGIMEN,


        [Description("@SDP_USUARIO_ACTUALIZA")]
        SDP_USUARIO_ACTUALIZA,

        [Description("@SDP_FECHA_ACTUALIZA")]
        SDP_FECHA_ACTUALIZA
    }

    public enum EnumCreditoArchivos
    {
        [Description("@SAC_NOMBRE_ARCHIVO")]
        NombreArchivo,

        [Description("@SOC_ID")]
        SolicitudCreditoId
    }
}
