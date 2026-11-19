using System;

namespace GA2.Domain.Entities.Cartera.Cierres
{
    public class RequestCierreDiarioCartera
    {
        public DateTime FechaCierre { get; set; }
        public bool Actualiza { get; set; }
        public Guid CreadoPor { get; set; }
    }
}
