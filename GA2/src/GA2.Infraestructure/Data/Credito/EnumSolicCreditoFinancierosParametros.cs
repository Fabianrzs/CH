using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data
{
    public enum EnumSolicCreditoFinancierosParametros
    {

        [Description("@SOF_ID")]
        SOF_ID,

        [Description("@SOC_ID")]
        SOC_ID,

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

    
    }
}
