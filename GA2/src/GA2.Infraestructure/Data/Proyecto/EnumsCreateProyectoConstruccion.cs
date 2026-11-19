using System.ComponentModel;

namespace GA2.Infraestructure.Data.Proyecto
{
    public enum EnumsCreateProyectoConstruccion
    {
        [Description("@PC_CONTRUCTORA_ID")]
        PC_CONTRUCTORA_ID,

        [Description("@PC_NOMBRE")]
        PC_NOMBRE,

        [Description("@PC_DIRECCION")]
        PC_DIRECCION,

        [Description("@PC_DEPARTAMENTO_ID")]
        PC_DEPARTAMENTO_ID,

        [Description("@PC_MUNICIPIO_ID")]
        PC_MUNICIPIO_ID,

        [Description("@PC_TELEFONO")]
        PC_TELEFONO,

        [Description("@PC_VALOR")]
        PC_VALOR,

        [Description("@PC_CUPOS")]
        PC_CUPOS,

        [Description("@PC_LATITUD")]
        PC_LATITUD,

        [Description("@PC_LONGITUD")]
        PC_LONGITUD,

        [Description("@PC_ESTADO_ID")]
        PC_ESTADO_ID,

        [Description("@PC_FECHA_CREACION")]
        PC_FECHA_CREACION,

        [Description("@PC_FECHA_VIGENCIA")]
        PC_FECHA_VIGENCIA,

        [Description("@PC_FECHA_ACTA_INICIO")]
        PC_FECHA_ACTA_INICIO,

        [Description("@PC_FECHA_ENTREGA")]
        PC_FECHA_ENTREGA,

        [Description("@PC_OBJETO")]
        PC_OBJETO,

        [Description("@PC_TIPO_CONTRATO_ID")]
        PC_TIPO_CONTRATO_ID,

        [Description("@PC_NUMERO_REGISTRO")]
        @PC_NUMERO_REGISTRO,

    }

    public enum EnumsLicenciaUrbanismoConstruccion
    {
        [Description("@LUP_RESOLUCION")]
        LUP_RESOLUCION,

        [Description("@LUP_CURADURIA")]
        LUP_CURADURIA,

        [Description("@LUP_DEPARTAMENTO_ID")]
        LUP_DEPARTAMENTO_ID,

        [Description("@LUP_MUNICIPIO_ID")]
        LUP_MUNICIPIO_ID,

        [Description("@LUP_DATE_VIGENCIA")]
        LUP_DATE_VIGENCIA,

        [Description("@LUP_TITULAR")]
        LUP_TITULAR,

        [Description("@LUP_DATE_RESOLUCION")]
        LUP_DATE_RESOLUCION,

        [Description("@LUP_MISMA_LICENCI")]
        LUP_MISMA_LICENCI,

        [Description("@PC_PROYECTO_ID")]
        PC_PROYECTO_ID
    }
    public enum EnumsLSupervisorProyecto
    {
        [Description("@SP_TIPO_DOCUMENTO")]
        SP_TIPO_DOCUMENTO,

        [Description("@SP_NUMERO_DOCUMENTO")]
        SP_NUMERO_DOCUMENTO,

        [Description("@SP_NOMBRE_APELLIDO")]
        SP_NOMBRE_APELLIDO,

        [Description("@SP_CARGO")]
        SP_CARGO,

        [Description("@SP_EMAIL")]
        SP_EMAIL,

        [Description("@SP_TELEFONO")]
        SP_TELEFONO,

        [Description("@PC_PROYECTO_ID")]
        PC_PROYECTO_ID
    }
    public enum EnumsLBeneficiarioProyecto
    {
        [Description("@BP_TIPO_DOCUMENTO")]
        BP_TIPO_DOCUMENTO,

        [Description("@BP_NUMERO_DOCUMENTO")]
        BP_NUMERO_DOCUMENTO,

        [Description("@BP_DIGITO_VEFICACION")]
        BP_DIGITO_VEFICACION,

        [Description("@BP_RAZON_SOCIAL")]
        BP_RAZON_SOCIAL,

        [Description("@BP_TIPO_CUENTA")]
        BP_TIPO_CUENTA,

        [Description("@BP_TIPO_ENTIDAD")]
        BP_TIPO_ENTIDAD,

        [Description("@BP_NUMERO_CUENTA")]
        BP_NUMERO_CUENTA,

        [Description("@BP_ACTIVIDAD_CUENTA")]
        BP_ACTIVIDAD_CUENTA,
        [Description("@BP_DIRECCION")]
        BP_DIRECCION,

        [Description("@PC_PROYECTO_ID")]
        PC_PROYECTO_ID
    }

    public enum EnumsPolizaProyecto
    {
        [Description("@PP_CONCEPTOPOLIZA_ID")]
        PP_CONCEPTOPOLIZA_ID,

        [Description("@PP_RADICADOPOLIZA")]
        PP_RADICADOPOLIZA,

        [Description("@PP_FECHAINICIO")]
        PP_FECHAINICIO,

        [Description("@PP_FECHAVIGENCIA")]
        PP_FECHAVIGENCIA,

        [Description("@PP_NUMEROPOLIZA")]
        PP_NUMEROPOLIZA,

        [Description("@PP_ASEGURADORA")]
        PP_ASEGURADORA,

        [Description("@PP_VALORPOLIZA")]
        PP_VALORPOLIZA,

        [Description("@PP_PORCENTAJE")]
        PP_PORCENTAJE,
        [Description("@PP_AJUSTADOR")]
        @PP_AJUSTADOR,
        [Description("@PP_DESCRIPCION")]
        PP_DESCRIPCION,

        [Description("@PC_PROYECTO_ID")]
        PC_PROYECTO_ID
    }
}
