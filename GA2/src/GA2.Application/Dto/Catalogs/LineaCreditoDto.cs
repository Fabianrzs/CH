using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Catalogs
{
    public class LineaCreditoDto
    {
        public int IdLineaCredito { get; set; }
        public string Descripcion { get; set; }
        public int TipoCredito { get; set; }
        public int? Tpa { get; set; }
        public int CuotaMinima { get; set; }
        public int CuotaMaxima { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
