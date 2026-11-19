using System.ComponentModel;

namespace GA2.Infraestructure.Data.Cartera.EnumValidarContinuidadCausacion
{
    public enum EnumTasaSimuladorParams
    {
        [Description("@SIM_ID")]
        IdSimulacion,

        [Description("@SIM_DESCRIPCION")]
        Descripcion,

        [Description("@SIM_TASA_EA")]
        TasaEA,

        [Description("@SIM_MINIMO_MESES_PLAZO")]
        MinimoPlazoMeses,

        [Description("@SIM_MAXIMO_MESES_PLAZO")]
        MaximoPlazoMeses,

        [Description("@SIM_ESTADO")]
        Estado,

        [Description("@SIM_CREADO_POR")]
        CreadoPor,

        [Description("@SIM_FECHA_CREACION")]
        FechaCreacion,

        [Description("@SIM_MODIFICADO_POR")]
        ModificadoPor,

        [Description("@SIM_FECHA_MODIFICACION")]
        FechaModificacion,

        [Description("@SIM_MIN_CANON_INICIAL")]
        MinCanonInicial,

        [Description("@SIM_MAX_CANON_INICIAL")]
        MaxCanonInicial,

        [Description("@SIM_MAX_OPC_COMPRA")]
        MaxOpcCompra,
        [Description("@SIM_TASA_NOMINAL_MENSUAL")]
        TasaNominalMensual,

        [Description("@SIM_TASA_HOGAR")]
        SeguroHogar,
    }

}
