using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    public enum EnumCreacionSolicitudCreditoParams
    {
        [Description("@SOC_ID")]
        SOC_ID,

        [Description("@SOP_PROPIEDAD_HORIZONTAL")]
        SOP_PROPIEDAD_HORIZONTAL,

        [Description("@SOP_TIPO_DEPOSITO")]
        SOP_TIPO_DEPOSITO,

        [Description("@SOP_TIPO_GARAGE")]
        SOP_TIPO_GARAGE,

        [Description("@SOC_FECHA_SOLICITUD")]
        SOC_FECHA_SOLICITUD,

        [Description("@SOC_NUMERO_SOLICITUD")]
        SOC_NUMERO_SOLICITUD,

        [Description("@TCR_ID")]
        TCR_ID,

        [Description("@DPD_ID")]
        DPD_ID,

        [Description("@DPC_ID")]
        DPC_ID,

        [Description("@SLS_ID")]
        SLS_ID,

        [Description("@TID_ID")]
        TID_ID,

        [Description("@CLI_IDENTIFICACION")]
        CLI_IDENTIFICACION,

        [Description("@SOC_DECLARACION_ORIGEN_FONDOS")]
        SOC_DECLARACION_ORIGEN_FONDOS,

        [Description("@SOC_AUTORIZA_USO_DATOS")]
        SOC_AUTORIZA_USO_DATOS,

        [Description("@SOC_AUTORIZA_CONSULTA_CENTRALES")]
        SOC_AUTORIZA_CONSULTA_CENTRALES,

        [Description("@SOC_DECLARACION_ASEGURABILIDAD")]
        SOC_DECLARACION_ASEGURABILIDAD,

        [Description("@SOC_ESTADO")]
        SOC_ESTADO,

        [Description("@SOC_CREADO_POR")]
        SOC_CREADO_POR,

        [Description("@SOC_FECHA_CREACION")]
        SOC_FECHA_CREACION,

        [Description("@SOP_ID")]
        SOP_ID,

        [Description("@SOP_ESTADO_INMUEBLE")]
        SOP_ESTADO_INMUEBLE,

        [Description("@SOP_TIPO_INMUEBLE")]
        SOP_TIPO_INMUEBLE,

        [Description("@SOP_TIENE_GARAJE")]
        SOP_TIENE_GARAJE,

        [Description("@SOP_TIENE_DEPOSITO")]
        SOP_TIENE_DEPOSITO,

        [Description("@SOP_CONJUNTO_CERRADO")]
        SOP_CONJUNTO_CERRADO,

        [Description("@TVV_ID")]
        TVV_ID,

        [Description("@SOP_AÑOS_CONSTRUCCION")]
        SOP_AÑOS_CONSTRUCCION,

        [Description("@SOP_MATRICULA_INMOBILIARIA1")]
        SOP_MATRICULA_INMOBILIARIA1,

        [Description("@SOP_MATRICULA_INMOBILIARIA2")]
        SOP_MATRICULA_INMOBILIARIA2,

        [Description("@SOP_MATRICULA_INMOBILIARIA3")]
        SOP_MATRICULA_INMOBILIARIA3,

        [Description("@SOP_VALOR_INMUEBLE")]
        SOP_VALOR_INMUEBLE,

        [Description("@SOP_DIRECCION_INMUEBLE")]
        SOP_DIRECCION_INMUEBLE,

        [Description("@SOP_BARRIO_INMUEBLE")]
        SOP_BARRIO_INMUEBLE,

        [Description("@SOP_LOCALIDAD_INMUEBLE")]
        SOP_LOCALIDAD_INMUEBLE,

        [Description("@SOP_PISO_INMUEBLE")]
        SOP_PISO_INMUEBLE,

        [Description("@SOP_POSEE_ASENSOR")]
        SOP_POSEE_ASENSOR,


        [Description("@DPD_ID_INMUEBLE")]
        DPD_ID_INMUEBLE,

        [Description("@DPC_ID_INMUEBLE")]
        DPC_ID_INMUEBLE,

        [Description("@SOP_VALOR_CREDITO")]
        SOP_VALOR_CREDITO,

        [Description("@SOP_PORC_FINANCIACION")]
        SOP_PORC_FINANCIACION,

        [Description("@SOP_PLAZO_FINANCIACION")]
        SOP_PLAZO_FINANCIACION,

        [Description("@SOP_FECHA_ENTREGA_INMUEBLE")]
        SOP_FECHA_ENTREGA_INMUEBLE,

        [Description("@TID_ID_VENDEDOR")]
        TID_ID_VENDEDOR,

        [Description("@SOP_NUMERO_DOCUMENTO_VENDEDOR")]
        SOP_NUMERO_DOCUMENTO_VENDEDOR,

        [Description("@SOP_NOMBRE_VENDEDOR")]
        SOP_NOMBRE_VENDEDOR,

        [Description("@SOP_DIRECCION_VENDEDOR")]
        SOP_DIRECCION_VENDEDOR,

        [Description("@DPD_ID_VENDEDOR")]
        DPD_ID_VENDEDOR,

        [Description("@DPC_ID_VENDEDOR")]
        DPC_ID_VENDEDOR,

        [Description("@SOP_CORREO_VENDEDOR")]
        SOP_CORREO_VENDEDOR,

        [Description("@SOP_TELEFONO_VENDEDOR")]
        SOP_TELEFONO_VENDEDOR,

        [Description("@SOC_CONVENIO_ASEGURADORA")]
        SOC_CONVENIO_ASEGURADORA,

        [Description("@SOC_CONVENIO_ASEGURADORA_HOGAR")]
        SOC_CONVENIO_ASEGURADORA_HOGAR,

        [Description("@ASE_ID")]
        ASE_ID,

        [Description("@ASE_ID_HOGAR")]
        ASE_ID_HOGAR,


        [Description("@SMC_NUMERO_DOCUMENTO")]
        SMC_NUMERO_DOCUMENTO,

        [Description("@SOC_SOCRE")]
        SOC_SOCRE,

        [Description("@SOC_ACTUALIZADO_POR")]
        SOC_ACTUALIZADO_POR,


        [Description("@SOC_FECHA_ACTUALIZA")]
        SOC_FECHA_ACTUALIZA,


        [Description("@TRS_ID")]
        TRS_ID,

        [Description("@ACT_ID")]
        ACT_ID,


        [Description("@TRS_OBSERVACION")]
        TRS_OBSERVACION,

        [Description("@ID_USUARIO_PERITO")]
        ID_USUARIO_PERITO,

        [Description("@TRS_MODIFICADO_POR")]
        TRS_MODIFICADO_POR,

        [Description("@TRS_FECHA_MODIFICACION")]
        TRS_FECHA_MODIFICACION,

        [Description("@ACT_OBSERVACION")]
        ACT_OBSERVACION,

        [Description("@ACT_FECHA_MODIFICACION")]
        ACT_FECHA_MODIFICACION,

        [Description("@ACT_MODIFICADO_POR")]
        ACT_MODIFICADO_POR,



        [Description("@Perfil")]
        Perfil,

        [Description("@Mensaje")]
        Mensaje,

        [Description("@Modulo")]
        Modulo,

        [Description("@UsuarioEnvia")]
        UsuarioEnvia,

        [Description("@USR_ID")]
        UsuarioID,
        
        [Description("@DPD_DEPARTAMENTO_NOTARIA")]        
        DepartamentoNotariaId,

        [Description("@DPC_CIUDAD_NOTARIA")]
        CiudadNotariaId,

        [Description("@FID_ACTIVO")]
        FID_ACTIVO,

        [Description("@FID_TIPO_DOCUMENTO")]
        FID_TIPO_DOCUMENTO,

        [Description("@FID_NUMERO_DOCUMENTO")]
        FID_NUMERO_DOCUMENTO,

        [Description("@FID_RAZON_SOCIAL")]
        FID_RAZON_SOCIAL,

        [Description("@FID_DIRECCION")]
        FID_DIRECCION,

        [Description("@FID_DEPARTAMENTO")]
        FID_DEPARTAMENTO,

        [Description("@FID_CIUDAD")]
        FID_CIUDAD,

        [Description("@FID_CORREO")]
        FID_CORREO,

        [Description("@FID_TELEFONO")]
        FID_TELEFONO

    }
}
