using System;

namespace GA2.Application.Dto.Cartera.Novedades
{
    public class RequestPagoNovedadDto
    {
        public Guid IdNovedad { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public Guid CreadoPor { get; set; }
        public bool AplicarPago { get; set; }
    }
}
