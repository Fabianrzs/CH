namespace GA2.Application.Dto
{
    public class GuardarArchivoCorregirDto
    {
        public int? Dct_id { get; set; }
	    public int? Corregir { get; set; }
        public int? Uej_Id { get; set; }
	    public string Dct_FechaCreacion { get; set; }
	    public int? Fin_Consecutivo { get; set; }
	    public int? Encabezado_Consecutivo { get; set; }
        public int? Codigo_fin_archivo { get; set; }
        public string Tipo_error { get; set; }
        public string Descripcion_error { get; set; }
        public string IndicadorControl { get; set; }
        public int? Tipo_Archivo { get; set; }

        //Cuerpo
        public int? ConsecutivoCuerpo { get; set; }
        public string IdentificacionCuerpo { get; set; }
        public string PrimerApellidoCuerpo { get; set; }
        public string PrimerNombreCuerpo { get; set; }
        public string SegundoApellidoCuerpo { get; set; }
        public string SegundoNombreCuerpo { get; set; }

    }
}