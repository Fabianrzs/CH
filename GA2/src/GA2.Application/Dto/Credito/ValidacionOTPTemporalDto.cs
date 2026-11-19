using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class ValidacionOTPTemporalDto
    {
        public Guid IdValidacionOTP { get; set; }
        public string Celular { get;set; }
        public string Identificacion { get; set; }  
        public int Intentos { get; set; }
        public bool EstadoValidacion { get; set; }
        public DateTime FechaValidacionOTP { get; set; }
    }
}
