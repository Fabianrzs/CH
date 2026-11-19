using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    public enum EnumReportesParams
    {
        [Description("@FECHA_APLICACION")]
        FechaAplicacion,

        [Description("@IDENTIFICACION")]
        Identificacion,

        [Description("@EBA_ID")]
        IdEmbargo,

        [Description("@ID_AFILIACION")]
        IdAfiliacion,

        [Description("@OPCION_REPORTE")]
        OpcionReporte,

        [Description("@ANIO_CIERRE")]
        AnioCierre,

        [Description("@MES_CIERRE")]
        MesCierre,

        [Description("@NRO_SECUENCIA")]
        NumeroSecuencia,

    }
}
