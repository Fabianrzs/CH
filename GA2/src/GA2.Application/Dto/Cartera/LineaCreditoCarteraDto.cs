using System.Collections.Generic;

namespace GA2.Application.Dto.Cartera
{
    public class LineaCreditoCarteraDto
    {
        public int TipoActorId { get; set; }
        public int Cuotas { get; set; }
        public int TipoCreditoId { get; set; }
        public IEnumerable<TipoActorDto> TipoActores { get; set; }
    }
}
