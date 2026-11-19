using System;

namespace GA2.Application.Dto.Cartera.Cierres.RequestCierreDiarioCartera
{
    public class RequestCierreDiarioCarteraDto
    {
        public DateTime FechaCierre { get; set; }
        public bool Actualiza { get; set; }
        public Guid CreadoPor { get; set; }
    }
}
