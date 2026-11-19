using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ReintegrosParamsDto
    {
        public string NumeroRadicado { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public int IdTipoReintegro { get; set; }
    }
}
