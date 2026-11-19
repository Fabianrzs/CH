namespace GA2.Application.Dto.Cartera.ParametersRequest
{
    public class ParametrosObtenerProcesoAplicacionPagosPorUEJDto
    {
        public int UnidadEjecutora { get; set; }
        public int? TipoCredito { get; set; }
        public int? Periodo { get; set; }
        public string Estado { get; set; }
    }
}
