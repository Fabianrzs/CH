using System;

namespace GA2.Application.Dto.Cartera.Seven
{
    public class RequestCarteraAdelantoCuotaSeven
    {
        public int tipoIdentificacion { get; set; }
        public string NumeroDocumentoCliente { get; set; }
        public long numeroCredito { get; set; } 
        public int tipoCredito { get; set; }
        public Guid cpgId { get; set; }
        public DateTime fechaCierre { get; set; }
    }
}
