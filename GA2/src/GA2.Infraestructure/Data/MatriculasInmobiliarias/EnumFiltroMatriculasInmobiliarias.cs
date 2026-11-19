using System.ComponentModel;

namespace GA2.Infraestructure.Data
{
    public enum EnumFiltroMatriculasInmobiliarias
    {

        [Description("@MAI_NUMERO_MATRICULA")]
        MAI_NUMERO_MATRICULA,

        [Description("@PRP_NUMERO_IDENTIFICACION")]
        PRP_NUMERO_IDENTIFICACION,

        [Description("@TID_ID")]
        TID_ID,

        [Description("@MAI_ID")]
        MAI_ID,

        [Description("@MAI_FECHA_REGISTRO")]
        MAI_FECHA_REGISTRO,

        [Description("@MAI_DIRECCION")]
        MAI_DIRECCION,

        [Description("@MAI_BLOQUEO")]
        MAI_BLOQUEO,

        [Description("@DPD_DEPARTAMENTO")]
        DPD_DEPARTAMENTO,

        [Description("@DPC_NOMBRE_CIUDAD")]
        DPC_NOMBRE_CIUDAD,

        [Description("@PRP_TIPO_IDENTIFICACION")]
        PRP_TIPO_IDENTIFICACION,

        [Description("@PRP_RAZON_SOCIAL")]
        PRP_NOMBRE_RAZON_SOCIAL,

        [Description("@PRP_PRIMER_NOMBRE")]
        PRP_PRIMER_NOMBRE,

        [Description("@PRP_SEGUNDO_NOMBRE")]
        PRP_SEGUNDO_NOMBRE,

        [Description("@PRP_PRIMER_APELLIDO")]
        PRP_PRIMER_APELLIDO,

        [Description("@PRP_SEGUNDO_APELLIDO")]
        PRP_SEGUNDO_APELLIDO,


        [Description("@CORREO")]
        CORREO,

        [Description("@TELEFONO")]
        TELEFONO,

        [Description("@OFC_ID")]
        OFC_ID,
        [Description("@OFC_DESCRIPCION")]
        OFC_DESCRIPCION,
        [Description("@NOMBRE_USUARIO")]
        NOMBRE_USUARIO,
        [Description("@NUMERO_DOCUMENTO")]
        NUMERO_DOCUMENTO,
        [Description("@ID_USUARIO")]
        ID_USUARIO,
        [Description("@CLI_ID")]
        CLI_ID,
        [Description("@MAI_FECHA_FIN")]
        MAI_FECHA_FINAL
    }


    public enum EnumNovedadMatriculasInmobiliarias
    {

        [Description("@NVM_ID")]
        NVM_ID,

        [Description("@NVM_FECHA_NOVEDAD")]
        NVM_FECHA_NOVEDAD,

        [Description("@MAI_ID")]
        MAI_ID,

        [Description("@TOP_ID")]
        TOP_ID,

        [Description("@PCS_ID")]
        PCS_ID,

        [Description("@TSN_ID")]
        TSN_ID,

        [Description("@CAN_ID")]
        CAN_ID,

        [Description("@SOL_ID")]
        SOL_ID,

        [Description("@NVM_CREATEDOPOR")]
        NVM_CREATEDOPOR,

        [Description("@NVM_CREATEDOPOR_ID")]
        NVM_CREATEDOPOR_ID,

        [Description("@NVM_OBSERVACIONES")]
        NVM_OBSERVACIONES,
        [Description("@NOMBRE_USUARIO")]
        NOMBRE_USUARIO,
        [Description("@DATE_NOVEDADES")]
         DATE_NOVEDADES,
        [Description("@DATE_REGISTRO")]
        DATE_REGISTRO,
        [Description("@ID_USUARIO")]
        ID_USUARIO,  
    }

    public enum EnumHistorialPropietarios
    {

        [Description("@MAI_ID")]
        MAI_ID,

        [Description("@PRP_ID")]
        PRP_ID,

        [Description("@PRP_NUMERO_IDENTIFICACION")]
        PRP_NUMERO_IDENTIFICACION,

        [Description("@PRP_NOMBRE_RAZON_SOCIAL")]
        PRP_NOMBRE_RAZON_SOCIAL,

        [Description("@TID_ID")]
        TID_ID,

        [Description("@CORREO")]
        CORREO,

        [Description("@TELEFONO")]
        TELEFONO,

        [Description("@PRP_PRIMER_NOMBRE")]
        PRP_PRIMER_NOMBRE,

        [Description("@PRP_SEGUNDO_NOMBRE")]
        PRP_SEGUNDO_NOMBRE,

        [Description("@PRP_PRIMER_APELLIDO")]
        PRP_PRIMER_APELLIDO,

        [Description("@PRP_SEGUNDO_APELLIDO")]
        PRP_SEGUNDO_APELLIDO,

        [Description("@NOMBRE_USUARIO")]
        NOMBRE_USUARIO,

        [Description("@DATE_CREATION")]
        DATE_CREATION,

        [Description("@USERID")]
        USERID,
    }
}
