using System;

namespace GA2.Application.Dto.Cartera.Cierres
{
    public class UltimoCierreDto
    {
        public int IdCierre { get; set; }
        public string TipoTransaccion { get; set; }
        public string Anio { get; set; }
        public string Mes { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string TipoCierre { get; set; }
        public int IdTipoCierre { get; set; }
    }
}
