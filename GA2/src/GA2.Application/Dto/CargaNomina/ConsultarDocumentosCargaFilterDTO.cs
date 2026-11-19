namespace GA2.Application.Dto
{
    /// <summary>
    /// Clase de los filtros de consulta para documentos de nomina cargados
    /// </summary>
    public class ConsultarDocumentosCargaFilterDTO
    {
        //public int? Frz_Id { get; set; }
	    public int? Uej_Id { get; set; }
	    public int? Tdc_Id { get; set; }
	    public int? Esd_Id { get; set; }
	    public string Dct_FechaCreacion { get; set; }
    }
}
