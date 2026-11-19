namespace GA2.Infraestructure.Data.Cartera.EnumValidarContinuidadCausacion
{
    /// <summary>
    /// Enum para nombres de parámetros del SP: ValidarContinuidadCausacionDiaria
    /// Usar con HelpersEnums.GetEnumDescription()
    /// </summary>
    public enum EnumValidarContinuidadCausacion
    {
        [System.ComponentModel.Description("@FECHA_CAUSACION")]
        FechaCausacion,

        [System.ComponentModel.Description("@TCR_ID")]
        TcrId,

        [System.ComponentModel.Description("@RESULTADO")]
        Resultado,

        [System.ComponentModel.Description("@MENSAJE_ERROR")]
        MensajeError
    }

}
