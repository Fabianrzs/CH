namespace GA2.Application.Dto
{
    public class CorreccionMasivaArchivoDto
    {
        public int? Dct_id { get; set; }
        public int? Uej_Id { get; set; }
	    public int? Dct_Periodo { get; set; }
	    public int? Valor { get; set; }
        public string Tipo_error { get; set; }
        public int idTipoArchivo { get; set; }
    }
}
