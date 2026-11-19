namespace GA2.Application.Dto.Haberes
{
    public class HaberesRequestDto
    {
        public string Reporte { get; set; }
        public string ClienteIdentificacion { get; set; }
        public int CuentaId { get; set; }
        public string Mes { get; set; }
        public string Anio { get; set; }
    }
    public class ReportHaberesRequestDto
    {
        public string customerIdentification { get; set; }
        public int accountId { get; set; }
        public string sUserQuery { get; set; }
    }

    public class ReporteResumenFinancieroDto
    {
        public string identification { get; set; }

    }

}
