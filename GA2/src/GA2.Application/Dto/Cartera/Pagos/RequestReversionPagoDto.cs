using System;

namespace GA2.Application.Dto.Cartera.Pagos
{
    public class RequestReversionPagoDto
    {
        public Guid IdPago { get; set; }
        public Guid CreadoPor { get; set; }
        public int Comprobante_Id { get; set; }
        public int Estado_Id { get; set; }
    }    
    
    public class RequestReversionPagoListaDto
    {
        public Guid CpgId { get; set; }
        public Guid CreadoPor { get; set; }

    }
}
