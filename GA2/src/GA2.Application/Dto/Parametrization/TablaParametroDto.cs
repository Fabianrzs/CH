using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Parametrization
{
    public class TablaParametroDto
    {
        public int IdTablaParametro { get; set; }
        public string TablaParametro { get; set; }
        public string Descripcion { get; set; }
    }

    public class ValorInmuebleDto
    {
        public int ANIO { get; set; }
        public int ID_CATEGORIA { get; set; }
       
    }

    public class ValorInmuebleResponseDto
    {
        public int VLI_ID { get; set; }

        public int VLI_ANIO { get; set; }

        public string CATEGORIA { get; set; } = string.Empty;

        public decimal VLI_VALOR { get; set; }

        public string VLI_TIPO { get; set; } = string.Empty;

        public string ESTADO_DESC { get; set; } = string.Empty;
    }
}
