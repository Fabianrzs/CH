using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class ValoresSimulacionAfiliadoDto
    {
        public int RiesgoMinimo { get; set; }
        public decimal ValorFinanciar { get; set; }
        public Guid SMS_ID { get; set; }
        public string NumeroDocumento { get; set; }
        public DateTime FechaRegistro {get; set;}
        public decimal ValorSectorReal { get; set;}
        public decimal ValorSectorFinanciero { get; set;}  
        public decimal Factor { get; set; }
    }
}
