using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito.Simulador.Comite
{
    public enum EnumIntegranteComiteCredito
    {
        [Description("@ICO_ID")]
        Ico_Id,

        [Description("@ICO_NOMBRE")]
        Ico_Nombre,

        [Description("@ICO_CARGO")]
        Ico_Cargo,

        [Description("@ICO_TIPO")]
        Ico_Tipo,

        [Description("@ICO_ESTADO")]
        Ico_Estado,

        [Description("@ICO_CREADO_POR")]
        Ico_Creado_Por,

        [Description("@ICO_FECHA_CREACION")]
        Ico_Fecha_Creacion,

        [Description("@ICO_ACTUALIZADO_POR")]
        Ico_Actualizado_Por,

        [Description("@ICO_FECHA_ACTUALIZA")]
        Ico_Fecha_Actualiza
    }
}
