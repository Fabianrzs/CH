using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class DataGenerarPagoDto
    {
        public Guid IdReintegro { get; set; }
        public int CodigoBanco { get; set; }
        public int TipoCuenta { get; set; }
        public double NumeroCuenta { get; set; }
        public string Direccion { get; set; }
    }
}
