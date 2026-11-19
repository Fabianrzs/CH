namespace GA2.Application.Dto
{
    public class GenerarPanillaPagoReintegro
    {
        public string Identificacion { get; set; }
        public int TipoIdentificacion { get; set; }
        public string Cdp { get; set; }
    }

    public class DataPanillaPagoReintegro
    {
        public string ApellidoNombre { get; set; }
        public int Identificacion { get; set; }
        public string UnidadEjecutora { get; set; }
        public string NIT { get; set; }
        public string Radicado { get; set; }
        public string NumeroPlanilla { get; set; }
        public string Cdp { get; set; }
        public string Banco { get; set; }
        public string NumeroCuenta { get; set; }
        public string Direccion { get; set; }
        public string TipoCuenta { get; set; }
    }
    public class DataUserPefil
    {
        public string NamePerfil { get; set; }

    }
}
