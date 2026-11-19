namespace GA2.Application.Dto.Cartera.ParametersRequest
{
    public class ParametrosObtenerProcesoAplicacionPagosDto
    {
        public int? IdProceso { get; set; }
        public int? IdUnidadEjecutora { get; set; }
        public int? IdTipoCredito { get; set; }
        public int? Periodo { get; set; }
        public string Estado { get; set; }

    }
}
