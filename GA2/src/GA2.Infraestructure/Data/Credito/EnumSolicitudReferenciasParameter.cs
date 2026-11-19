using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumSolicitudReferenciasParameter
    {

        [Description("@SOR_ID")]
        SOR_ID,

        [Description("@SOC_ID")]
        SOC_ID,

        [Description("@SOR_TIPO_REFERENCIA")]
        SOR_TIPO_REFERENCIA,

        [Description("@SOR_PRIMER_APELLIDO")]
        SOR_PRIMER_APELLIDO,

        [Description("@SOR_SEGUNDO_APELLIDO")]
        SOR_SEGUNDO_APELLIDO,

        [Description("@SOR_NOMBRES")]
        SOR_NOMBRES,

        [Description("@SOR_PARENTESCO")]
        SOR_PARENTESCO,

        [Description("@SOR_RELACION")]
        SOR_RELACION,

        [Description("@SOR_DIRECCION")]
        SOR_DIRECCION,

        [Description("@DPD_ID")]
        DPD_ID,

        [Description("@DPC_ID")]
        DPC_ID,

        [Description("@SOR_TELEFONO")]
        SOR_TELEFONO,

        [Description("@SOR_CELULAR")]
        SOR_CELULAR,

        [Description("@SOR_CREADO_POR")]
        SOR_CREADO_POR,

        [Description("@SOR_FECHA_CREACION")]
        SOR_FECHA_CREACION,


        [Description("@SOF_ID ")]
        SOF_ID,

        [Description("@SOF_DIRECCION_VIVIENDA")]
        SOF_DIRECCION_VIVIENDA,

        [Description("@SOF_MATRICULA_VIVIENDA")]
        SOF_MATRICULA_VIVIENDA,

        [Description("@SOF_VALOR_COMERCIAL_VIVIENDA")]
        SOF_VALOR_COMERCIAL_VIVIENDA,

        [Description("@SOF_MARCA_VEHICULO")]
        SOF_MARCA_VEHICULO,

        [Description("@SOF_PLACA_VEHICULO")]
        SOF_PLACA_VEHICULO,

        [Description("@SOF_VALOR_COMERCIAL_VEHICULO")]
        SOF_VALOR_COMERCIAL_VEHICULO,

        [Description("@SOF_ACTUALIZADO_POR")]
        SOF_ACTUALIZADO_POR,

        [Description("@SOF_FECHA_ACTUALIZA")]
        SOF_FECHA_ACTUALIZA,

        [Description("@IND")]
        IND,

        [Description("@SOR_REFERENCIA")]
        SOR_REFERENCIA,

        [Description("@TIPO_REFERENCIA")]
        TIPO_REFERENCIA,

        
    }
}
