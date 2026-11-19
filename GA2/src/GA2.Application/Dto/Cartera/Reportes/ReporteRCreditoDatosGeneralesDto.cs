using System.Collections.Generic;
using System.Linq;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteRCreditoDatosGeneralesDto
    {
        public int IdModadlidad { get; set; }
        public int? IdModalidadPrincipal { get; set; }
        public string Modalidad { get; set; }
        public decimal? SaldoCapitalMesAnterior { get; set; }
        public decimal? SaldoDesembolsos { get; set; }
        public int? CantidadDesembolsos { get; set; }
        public decimal? SaldoRecaudos { get; set; }
        public int? CantidadRecaudos { get; set; }
        public decimal? SaldoCancelaciones { get; set; }
        public int? CantidadCancelaciones { get; set; }
        public decimal? SaldoCastigos { get; set; }
        public int? CantidadCastigos { get; set; }
        public decimal? SaldoRestructuraciones { get; set; }
        public int? CantidadRestructuraciones { get; set; }

        public IEnumerable<ReporteRCreditoDatosGeneralesDto> SubModalidades { get; set; } = Enumerable.Empty<ReporteRCreditoDatosGeneralesDto>();

        public ReporteRCreditoDatosGeneralesDto()
        {

        }

        public ReporteRCreditoDatosGeneralesDto(string modalidad)
        {
            Modalidad = modalidad;
        }

        public static implicit operator ReporteRCreditoDatosGeneralesDto(string modalidad)
        {
            return new ReporteRCreditoDatosGeneralesDto(modalidad);
        }
    }
}