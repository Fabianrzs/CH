namespace GA2.Application.Dto.Cartera.Reportes
{
    public class DatosReporte
    {
        public ArchivoReporte Archivo { get; set; }

        public long? NumeroCredito { get; set; }
        public string TipoCredito { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string Email { get; set; }
        public string UnidadEjecutora { get; set; }

    }
}
