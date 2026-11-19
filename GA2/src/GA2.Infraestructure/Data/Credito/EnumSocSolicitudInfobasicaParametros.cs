using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    public enum EnumSocSolicitudInfobasicaParametros
    {

        [Description("@SOB_ID")]
        SOB_ID,
        [Description("@SOC_ID")]
        SOC_ID,
        [Description("@TID_ID")]
        TID_ID,
        [Description("@SOB_NUMERO_DOCUMENTO")]
        SOB_NUMERO_DOCUMENTO,
        [Description("@DPD_ID_EXP")]
        DPD_ID_EXP,
        [Description("@SOB_FECHA_EXPEDICION")]
        SOB_FECHA_EXPEDICION,
        [Description("@DPC_ID_EXP")]
        DPC_ID_EXP,
        [Description("@SOB_PRIMER_APELLIDO")]
        SOB_PRIMER_APELLIDO,
        [Description("@SOB_SEGUNDO_APELLIDO")]
        SOB_SEGUNDO_APELLIDO,
        [Description("@SOB_PRIMER_NOMBRE")]
        SOB_PRIMER_NOMBRE,
        [Description("@SOB_SEGUNDO_NOMBRE")]
        SOB_SEGUNDO_NOMBRE,
        [Description("@SOB_FECHA_NACIMIENTO")]
        SOB_FECHA_NACIMIENTO,
        [Description("@DPD_ID_NACIMIENTO")]
        DPD_ID_NACIMIENTO,
        [Description("@DPC_ID_NACIMIENTO")]
        DPC_ID_NACIMIENTO,
        [Description("@SOB_SEXO")]
        SOB_SEXO,
        [Description("@SOB_ESTADO_CIVIL")]
        SOB_ESTADO_CIVIL,
        [Description("@SOB_NIVEL_EDUCACION")]
        SOB_NIVEL_EDUCACION,
        [Description("@FRZ_ID")]
        FRZ_ID,
        [Description("@CTG_ID")]
        CTG_ID,
        [Description("@GRD_ID")]
        GRD_ID,
        [Description("@DPD_ID_RESIDENCIA")]
        DPD_ID_RESIDENCIA,
        [Description("@DPC_ID_RESIDENCIA")]
        DPC_ID_RESIDENCIA,
        [Description("@SOB_DIRECCION_RESIDENCIA")]
        SOB_DIRECCION_RESIDENCIA,
        [Description("@SOB_CORREO_PERSONAL")]
        SOB_CORREO_PERSONAL,
        [Description("SOB_TELEFONO_RESIDENCIA")]
        SOB_TELEFONO_RESIDENCIA,
        [Description("@SOB_CELULAR")]
        SOB_CELULAR,
        [Description("@DPD_ID_OFICINA")]
        DPD_ID_OFICINA,
        [Description("@DPC_ID_OFICINA")]
        DPC_ID_OFICINA,
        [Description("@SOB_DIRECCION_OFICINA")]
        SOB_DIRECCION_OFICINA,
        [Description("@SOB_CORREO_INSTITUCIONAL")]
        SOB_CORREO_INSTITUCIONAL,
        [Description("@SOB_TELEFONO_OFICINA")]
        SOB_TELEFONO_OFICINA,
        [Description("@ACC_ID")]
        ACC_ID,
        [Description("@PRF_ID")]
        PRF_ID,
        [Description("@SOB_CREADO_POR")]
        SOB_CREADO_POR,

        [Description("@SOB_FECHA_CREACION")]
        SOB_FECHA_CREACION,

      

        [Description("@SOB_TIENE_TRANSACCION_ME")]
        SOB_TIENE_TRANSACCION_ME,
        [Description("@SOB_TIPO_TRANSACCION_ME")]
        SOB_TIPO_TRANSACCION_ME,
        [Description("@SOB_TIPO_PRODUCTO_ME")]
        SOB_TIPO_PRODUCTO_ME,
        [Description("@SOB_BANCO_TRANSACCION_ME")]
        SOB_BANCO_TRANSACCION_ME,

        [Description("@SOB_NUMERO_PRODUCTO_ME")]
        SOB_NUMERO_PRODUCTO_ME,
        [Description("@SOB_SALDO_PROMEDIO_CUENTAS_ME")]
        SOB_SALDO_PROMEDIO_CUENTAS_ME,
        [Description("@SOB_MONEDA_TRANSACCION_ME")]
        SOB_MONEDA_TRANSACCION_ME,

        [Description("@DPP_ID_ME")]
        DPP_ID_ME,
        [Description("@DPD_ID_ME")]
        DPD_ID_ME,
        [Description("@SOB_ACTUALIZADO_POR")]
        SOB_ACTUALIZADO_POR,
        [Description("@SOB_FECHA_ACTUALIZA")]
        SOB_FECHA_ACTUALIZA,
     

    }
}
