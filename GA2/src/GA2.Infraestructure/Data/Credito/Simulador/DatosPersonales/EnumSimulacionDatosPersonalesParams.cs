using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GA2.Infraestructure.Data
{
    public enum EnumSimulacionDatosPersonalesParams
    {
        [Description("@SDP_NUMERO_DOCUMENTO")]
        NumeroDocumento,

        [Description("@IND")]
        Indicador,

        [Description("@SDP_VALOR_INMUEBLE")]
        ValorInmueble,

        [Description("@TVV_ID")]
        TipoVivienda,

        [Description("@DPD_ID_INMUEBLE")]
        DepartamentoInmueble,

        [Description("@DPC_ID_INMUEBLE")]
        CiudadInmueble,

        [Description("@SDP_VALOR_AVALUO_INMUEBLE")]
        ValorAvaluoInmueble,

        [Description("@USR_ID")]
        UsuarioId,

        [Description("@SDP_OPCION_COMPRA_CARTERA")]
        OpcionCompraCartera,
        
        [Description("@SDP_VALOR_PRESTAMO_COMPRA_CARTERA")]
        ValorPrestamoCartera,

        [Description("@SDP_NUMERO_OBLIGACION_COMPRA_CARTERA")]
        NumeroObligacionCompraCartera

    }
}
