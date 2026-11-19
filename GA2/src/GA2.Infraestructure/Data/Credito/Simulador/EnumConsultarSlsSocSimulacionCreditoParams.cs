using System.ComponentModel;

namespace GA2.Infraestructure.Data.Credito.Simulador
{
    public enum EnumConsultarSlsSocSimulacionCreditoParams
    {
        [Description("@SLS_ID")]
        idSolicitudCredito,
        [Description("@NRO_DOCUMENTO")]
        nroDocumento,
    }
}
