using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.BUA
{
    public class AnotacionClienteDto
    {
        public int AnotacionClienteId { get; set; }
        public string Anotacion { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaAnotacion { get; set; }
        public int ClienteId { get; set; }
    }
}
