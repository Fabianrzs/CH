using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Reintegros
{
    public class ParametroObtenerReintegroDto
    {
        public Guid idReintegros { get; set; }
        public int? IdCliente { get; set; }
    }
}
