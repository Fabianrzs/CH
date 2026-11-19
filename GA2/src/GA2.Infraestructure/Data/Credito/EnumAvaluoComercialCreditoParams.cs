using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Infraestructure.Data.Credito
{
    public enum EnumAvaluoComercialCreditoParams
    {
        [Description("@SOC_ID")]
        SOC_ID,

        [Description("@AVC_CLIENTE")]
        AVC_CLIENTE,

        [Description("@AVC_NUMERO_DOCUMENTO")]
        AVC_NUMERO_DOCUMENTO,

        [Description("@AVC_DIRECCION")]
        AVC_DIRECCION,

        [Description("@AVC_SECTOR")]
        AVC_SECTOR,

        [Description("@AVC_BARRIO")]
        AVC_BARRIO,

        [Description("@AVC_ID_DPD")]
        AVC_ID_DPD,

        [Description("@AVC_ID_DPC")]
        AVC_ID_DPC,

        [Description("@AVC_FECHA_INFORME")]
        AVC_FECHA_INFORME,

        [Description("@AVC_VETUSTEZ")]
        AVC_VETUSTEZ,

        [Description("@AVC_FECHA_PAGO_AVALUO")]
        AVC_FECHA_PAGO_AVALUO,

        [Description("@AVC_FECHA_VISITA")]
        AVC_FECHA_VISITA,

        [Description("@AVC_PROPIETARIO")]
        AVC_PROPIETARIO,

        [Description("@AVC_ESCRITURA")]
        AVC_ESCRITURA,

        [Description("@AVC_NOTARIA")]
        AVC_NOTARIA,

        [Description("@AVC_FECHA_ESCRITURA")]
        AVC_FECHA_ESCRITURA,

        [Description("@AVC_DPD_ID_ESCRITURA")]
        AVC_DPD_ID_ESCRITURA,

        [Description("@AVC_DPC_ID_ESCRITURA")]
        AVC_DPC_ID_ESCRITURA,

        [Description("@AVC_CEDULA_CATASTRAL")]
        AVC_CEDULA_CATASTRAL,

        [Description("@AVC_LICENCIA_CONSTRUCCION")]
        AVC_LICENCIA_CONSTRUCCION,

        [Description("@AVC_NOMBRE_CONJUNTO")]
        AVC_NOMBRE_CONJUNTO,

        [Description("@AVC_COEFICIENTE_COPROPIEDAD")]
        AVC_COEFICIENTE_COPROPIEDAD,

        [Description("@AVC_DISTRIBUCION_INMUEBLE")]
        AVC_DISTRIBUCION_INMUEBLE,

        [Description("@AVC_CARACTERISTICAS_ADICIONALES")]
        AVC_CARACTERISTICAS_ADICIONALES,

        [Description("@AVC_DESCRIPCION_DEPENDENCIA")]
        AVC_DESCRIPCION_DEPENDENCIA,

        [Description("@AVC_GARAJE")]
        AVC_GARAJE,

        [Description("@AVC_DESCRIPCION_GARAJE")]
        AVC_DESCRIPCION_GARAJE,

        [Description("@AVC_SALA_AREA_CONSTRUIDA")]
        AVC_SALA_AREA_CONSTRUIDA,

        [Description("@AVC_AREA_PRIVADA")]
        AVC_AREA_PRIVADA,

        [Description("@AVC_CUADRO_VALORACION_INMUEBLE")]
        AVC_CUADRO_VALORACION_INMUEBLE
    }
}
