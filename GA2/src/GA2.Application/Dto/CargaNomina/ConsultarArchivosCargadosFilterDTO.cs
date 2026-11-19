namespace GA2.Application.Dto
{
    public class ConsultarArchivosCargadosFilterDTO
    {
        //public int? Frz_Id { get; set; }
        public int? Dct_Id { get; set; }
        public int? Uej_Id { get; set; }
	    public int? Tdc_Id { get; set; }
	    public int? Esd_Id { get; set; }
	    public int? Dct_FechaCreacion { get; set; }
	    public string FechaInicioCarga { get; set; }
	    public string FechaFinalCarga { get; set; }
	    public string NombreArchivo { get; set; }
    }
}
