namespace GA2.Application.Dto
{
    /// <summary>
    /// DTO documento carga de archivo
    /// </summary>
    /// <author>Federico Marin</author>
	/// <date>22/06/2023</date>
    public class EstadoArchivoDto
    {
        public int ID { get; set; }
        public string DESCRIPCION { get; set; }
        public string TIPO { get; set; }
        public int ESTADO { get; set; }
        public string CONCEPTO { get; set; }

    }
}
