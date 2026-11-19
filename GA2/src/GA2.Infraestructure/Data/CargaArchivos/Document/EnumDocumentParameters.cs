using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    /// <summary>
    /// Lista de parametros de documento
    /// </summary>
    /// <author>Nicolas Florez Sarrazola</author>
	/// <date>12/04/2021</date>
    public enum EnumDocumentParameters
    {
        [Description("@DCT_ID")]
        DCT_ID,

        [Description("@TDC_ID")]
        TDC_ID,

        [Description("@DCT_NOMBRE")]
        DCT_NOMBRE,

        [Description("@DCT_FECHA_INICIAL")]
        DCT_FECHA_INICIAL,

        [Description("@ESD_ID")]
        ESD_ID, 
        
        [Description("@EST_ID")]
        EST_ID,

        [Description("@CED_ID")]
        CED_ID,

        [Description("@DCT_FECHA_FINAL")]
        DCT_FECHA_FINAL,

        [Description("@DCT_ID_ANULA")]
        DCT_ID_ANULA,

        [Description("@DCT_ALERTA")]
        DCT_ALERTA,

        [Description("@DCT_CREADOPOR")]
        DCT_CREADOPOR,

        [Description("@DCT_FECHACREACION")]
        DCT_FECHACREACION,

        [Description("@DCT_FECHAMODIFICACION")]
        DCT_FECHAMODIFICACION,

        [Description("@DCT_MODIFICADOPOR")]
        DCT_MODIFICADOPOR,

        [Description("@UEJ_ID")]
        UEJ_ID,

        //[Description("@FRZ_ID")]
        //FRZ_ID,

        [Description("@TAR_ID")]
        TAR_ID,
        
        [Description("@EAN_CONSECUTIVO")]
        EAN_CONSECUTIVO,

        [Description("@CAN_IDENTIFICACION")]
        CAN_IDENTIFICACION,

        [Description("@CAN_PRIMER_NOMBRE")]
        CAN_PRIMER_NOMBRE,

        [Description("@CAN_SEGUNDO_NOMBRE")]
        CAN_SEGUNDO_NOMBRE,

        [Description("@CAN_PRIMER_APELLIDO")]
        CAN_PRIMER_APELLIDO,

        [Description("@CAN_SEGUNDO_APELLIDO")]
        CAN_SEGUNDO_APELLIDO,

        [Description("@CAN_CONSECUTIVO")]
        CAN_CONSECUTIVO,

        [Description("@FIN_CONSECUTIVO")]
        FIN_CONSECUTIVO,

        [Description("@CODIGO_FIN")]
        CODIGO_FIN,

        [Description("@FECHA_INICIO_CARGA")]
        FECHA_INICIO_CARGA,

        [Description("@FECHA_FINAL_CARGA")]
        FECHA_FINAL_CARGA,

        [Description("@NOMBRE_ARCHIVO")]
        NOMBRE_ARCHIVO,

        [Description("@VALOR")]
        VALOR,

        [Description("@DCT_REGISTRO")]
        DCT_REGISTRO,

        [Description("@FAN_INDICADOR_CONTROL")]
        FAN_INDICADOR_CONTROL,

        [Description("@SNMN_ESTADO_SOLICITUD")]
        SNMN_ESTADO_SOLICITUD,

        [Description("@SNMN_FECHA_INICIAL")]
        SNMN_FECHA_INICIAL,

        [Description("@SNMN_FECHA_FINAL")]
        SNMN_FECHA_FINAL,

        [Description("@SNMN_OBSERVACION")]
        SNMN_OBSERVACION,

        [Description("@SNMN_AUTORIZADOPOR")]
        SNMN_AUTORIZADOPOR,

        [Description("@SNMN_FECHACREACION")]
        @SNMN_FECHACREACION,

        [Description("@SNMN_PROCESADOPOR")]
        SNMN_PROCESADOPOR,

        [Description("@SNMN_APROBADOPOR")]
        SNMN_APROBADOPOR,

        [Description("@SNMN_ESTADO")]
        SNMN_ESTADO,

        [Description("@SNMN_FECHAMODIFICACION")]
        SNMN_FECHAMODIFICACION,

        [Description("@SNMN_RADICADO")]
        SNMN_RADICADO,

        [Description("@SNMN_MONTO")]
        SNMN_MONTO,

        [Description("@SNMN_PERIODO")]
        SNMN_PERIODO,

        [Description("@SNMN_REGISTROS")]
        @SNMN_REGISTROS,

        [Description("@SNMN_TIPO_ARCHIVO")]
        SNMN_TIPO_ARCHIVO,

        [Description("@SNMN_NOMBRE_ARCHIVO")]
        SNMN_NOMBRE_ARCHIVO,

        [Description("@SNMN_ID")]
        SNMN_ID,

        [Description("@SNMN_NUMERO_SOLICITUD")]
        SNMN_NUMERO_SOLICITUD,

        [Description("@USR_USUARIO")]
        USUARIO_ID,

        [Description("@CRN_ID")]
        CAUSAL_RECHAZO_ID,
        [Description("@ESTADO")]
        ESTADO,
        [Description("@UEJ_USUARIO")]
        UEJ_USUARIO,
        [Description("@NRN_TIPO_ARCHIVO")]
        NRN_TIPO_ARCHIVO,
        [Description("@NRN_PERIODO")]
        NRN_PERIODO,
        [Description("@ERRORES")]
        ERRORES

    }
}

