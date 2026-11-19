using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ObtenerOrdenDescuentosDto
    {
        public int conceptoId { get; set; }
        public int codPreProducto { get; set; }
        public string concepto { get; set; }
        public string preProducto { get; set; }
    }
}
