using System;

namespace GA2.Application.Dto.Cartera.Seven
{
    public class ResponseNovedadRediferido
    {
        public int TipoIdentificacion { get; set; }
        public string NumeroDocumentoCliente { get; set; }
        public long NumeroCredito { get; set; }
        public int TipoCredito { get; set; }
        public int CuotaCredito { get; set; }
        public Guid CreditoId { get; set; }
        public DateTime FechaCierre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
