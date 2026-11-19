namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteRCreditoDatosProvision
    {
        public string Nombre { get; set; }
        public object Normativa { get; set; }
        public object Adicional { get; set; }
        public TipoValorCelda TipoValor { get; set; }

        public ReporteRCreditoDatosProvision()
        {

        }

        public ReporteRCreditoDatosProvision(string nombre)
        {
            Nombre = nombre;
        }

        public static implicit operator ReporteRCreditoDatosProvision(string modalidad)
        {
            return new ReporteRCreditoDatosProvision(modalidad);
        }
    }

}