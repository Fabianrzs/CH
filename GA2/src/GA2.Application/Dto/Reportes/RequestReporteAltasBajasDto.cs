namespace GA2.Application.Dto.Reportes
{
    public class RequestReporteAltasBajasDto
    {
        public int IdUnidadEjecutora { get; set; }
        public TipoReporteUnidades TipoReporte { get; set; }
        public int PeriodoGeneracion { get; set; }
    }

    public class ReporteAltasBajasParametersDto
    {
        public int IdUnidadEjecutora { get; set; }
        public int TipoReporte { get; set; }
        public int PeriodoGeneracion { get; set; }
    }

    public enum TipoReporteUnidades
    {
        BAJAS = 0,
        ALTAS = 1,
        ALTAS_Y_BAJAS = 2
    }
}
