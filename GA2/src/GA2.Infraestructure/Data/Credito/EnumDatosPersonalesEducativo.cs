using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumDatosPersonalesEducativo
    {
        [Description("@DPE_ID")] DPE_ID,
        [Description("@DPE_TIPO_DOCUMENTO")] DPE_TIPO_DOCUMENTO,
        [Description("@DPE_NUMERO_DOCUMENTO")] DPE_NUMERO_DOCUMENTO,
        [Description("@DPE_PRIMER_NOMBRE_ESTUDIANTE")] DPE_PRIMER_NOMBRE_ESTUDIANTE,
        [Description("@DPE_SEGUNDO_NOMBRE_ESTUDIANTE")] DPE_SEGUNDO_NOMBRE_ESTUDIANTE,
        [Description("@DPE_PRIMER_APELLIDO_ESTUDIANTE")] DPE_PRIMER_APELLIDO_ESTUDIANTE,
        [Description("@DPE_SEGUNDO_APELLIDO_ESTUDIANTE")] DPE_SEGUNDO_APELLIDO_ESTUDIANTE,
        [Description("@DPE_DIRECCION_ESTUDIANTE")] DPE_DIRECCION_ESTUDIANTE,
        [Description("@DPE_VALOR_CREDITO")] DPE_VALOR_CREDITO,
        [Description("@DPE_PERIODO_LECTIVO")] DPE_PERIODO_LECTIVO,
        [Description("@FK_ENE_ID")] FK_ENE_ID,
        [Description("@ENE_DPD_ID")] ENE_DPD_ID,
        [Description("@ENE_DPC_ID")] ENE_DPC_ID,
        [Description("@SLS_ID")] SLS_ID,
        [Description("@DPE_NUMERO_TITULAR")] DPE_NUMERO_IDENTIFICACION_TITULAR
    }

    public enum EnumEntidadEducativa
    {
        [Description("@BEN_ID")]
        BEN_ID,

        [Description("@BEN_TIPO_DOCUMENTO")]
        BEN_TIPO_DOCUMENTO,

        [Description("@BEN_NIT")]
        BEN_NIT,

        [Description("@BEN_RAZON_SOCIAL_ENTIDAD")]
        BEN_RAZON_SOCIAL_ENTIDAD,

        [Description("@BEN_DIRECCION")]
        BEN_DIRECCION,

        [Description("@BEN_DEPARTAMENTO")]
        BEN_DEPARTAMENTO,

        [Description("@BEN_CIUDAD")]
        BEN_CIUDAD,

        [Description("@BEN_CORREO")]
        BEN_CORREO,

        [Description("@BEN_TELEFONO")]
        BEN_TELEFONO,
        
        [Description("@BEN_TIPO_CUENTA")]
        BEN_TIPO_CUENTA ,
        
        [Description("@BEN_NUMERO_CUENTA")]
        BEN_NUMERO_CUENTA,
        
        [Description("@BEN_BANCO")]
        BEN_BANCO ,
        
        [Description("@BEN_FECHA_LIMITE")]
        BEN_FECHA_LIMITE ,
        
        [Description("@SLS_ID")]
        SLS_ID ,
        
        [Description("@DPE_NUMERO_TITULAR")]
        DPE_NUMERO_TITULAR 
    }
}
