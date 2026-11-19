using Microsoft.AspNetCore.Http;
using System;

namespace GA2.Application.Dto.Cartera.Comprobantes.BalanceCartera
{
    public class ActualizarPlanPagoRequestDto
    {
        public IFormFile File { get; set; }
        public DateTime FechaCorte { get; set; }
        public int TipoCredito { get; set; }

    }

}

