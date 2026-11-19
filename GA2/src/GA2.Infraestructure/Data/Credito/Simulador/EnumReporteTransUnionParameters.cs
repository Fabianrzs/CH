using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito.Simulador
{
    public enum EnumReporteTransUnionParameters
    {
        [Description("@RTU_FECHA_CONSULTA")]
        RTU_FECHA_CONSULTA,

        [Description("@RTU_NUMERO_IDENTIFICACION")]
        RTU_NUMERO_IDENTIFICACION,

        [Description("@RTU_NOMBRE")]
        RTU_NOMBRE,

        [Description("@RTU_DEPARTAMENTO")]
        RTU_DEPARTAMENTO,

        [Description("@RTU_CIUDAD")]
        RTU_CIUDAD,

        [Description("@RTU_TIPO_CREDITO")]
        RTU_TIPO_CREDITO,

        [Description("@RTU_VALOR_INMUEBLE")]
        RTU_VALOR_INMUEBLE,

        [Description("@RTU_ESTADO")]
        RTU_ESTADO,

        [Description("@RTU_MOTIVO")]
        RTU_MOTIVO,

        [Description("@RTU_CREADO_POR")]
        RTU_CREADO_POR,

        [Description("@FECHA_DESDE")]
        FECHA_DESDE,

        [Description("@FECHA_HASTA")]
        FECHA_HASTA,
    }
}
