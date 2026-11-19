using System;

namespace GA2.Domain.Entities
{
    /// <summary>
	/// Lista de TramitesPendientes
	/// </summary>
	/// <date>10/05/2021</date>
    public class TramitesPendientes
    {
        public Guid SOC_ID { get; set; }
        public int TCR_ID { get; set; }
        public string TCR_DESCRIPCION { get; set; }
        public int SOC_NUMERO_SOLICITUD { get; set; }
        public string CLI_IDENTIFICACION { get; set; }
        public string NOMBRES{ get; set; }
        public double VALOR_CREDITO{ get; set; }
        public double  PLAZO { get; set; }
        public double TASA_EA { get; set; }
    }
}
