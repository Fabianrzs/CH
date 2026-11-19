using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.BUA
{
    public class ReporteTotalesDto
    {
        public int UEJ_ID { get; set; }
        public string? NombreUnidadEjecutora { get; set; }
        public int ConceptoID { get; set; }
        public string? NombreConcepto { get; set; }
        public decimal? TotalConcepto { get; set; }
        public decimal? TotalGeneralConcepto { get; set; }
        public string? TipoCuenta { get; set; }
    }

}
