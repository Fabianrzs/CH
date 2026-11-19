using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    public enum EnumSolicitudSimulacionParameters
    {

        [Description("@SLS_ID")]
        Id,

        [Description("@SLS_FECHA_SOLICITUD")]
        FechaSolicitud,

        [Description("@TCR_ID")]
        TcrId,

        [Description("@SLS_NUMERO_PREAPROBADO")]
        NumeroPreAprobado,

        [Description("@SLS_PROBLEMA_EMAIL")]
        ProblemaEmail,

        [Description("@SLS_ENVIO_NOTIFICACION")]
        EnvioNotificacion,

        [Description("@SLS_RUTA_PDF_RESUMEN")]
        RutaPdf,

        [Description("@SLS_PORC_EXTRAPRIMA ")]
        PorcExtraprima,

        [Description("@SLS_ESTADO")]
        Estado,

        [Description("@SLS_USUARIO_ACTUALIZA")]
        UsuarioActualiza,

        [Description("@SLS_FECHA_ACTUALIZA")]
        FechaActualiza,

        [Description("@SLS_NUMERO_RADICADO")]
        NumRadicadoGD,

        [Description("@SLS_NUMERO_RADICADO_FLUJO_83")]
        NumRadicado83,

        [Description("@SLS_RECORD_ID")]
        RecordIdGD,

        [Description("@NUMERO_DOCUMENTO")]
        numeroDocumento,

        [Description("@PA_ID")]
        Pa_Id,

        [Description("@SLS_SCORE_TU")]
        Sls_Score,

        [Description("@SLS_PORC_ENDEUDAMIENTO_SCH")]
        EndeudamientoSCH,

        [Description("@SLS_PORCENDEUDAMIENTO_CCH")]
        EndeudamientoCCH,

        [Description("@NUMERO_FOLIOS")]
        NumerosFolios,

        [Description("@MEDIOS_CREDITO_VIVIENDA")]
        MediosCreditoVivienda,

        [Description("@MEDIOS_RECEPCION")]
        MedioRecepcion,

        [Description("@SOC_ID")]
        SOC_ID,
        [Description("@DCS_RUTA_IMAGEN")]
        DCS_RUTA_IMAGEN,
        [Description("@DCS_APROBACION")]
        DCS_APROBACION,

        [Description("@SMC_ID")]
        SimulacionCliente,

        [Description("@SLS_SIMULADOR_DESC")]
        SimuladorDescripcion



    }
}
