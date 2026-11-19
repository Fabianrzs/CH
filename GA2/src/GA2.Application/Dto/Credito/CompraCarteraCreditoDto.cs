using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito
{
    public class CompraCarteraCreditoDto
    {
        public Guid CCC_ID { get; set; }
        public decimal ValorCompraCartera { get; set; }
        public decimal Cuota { get; set; }
        public decimal Plazo { get; set; }
        public decimal ValorFinalPrestamoCartera { get; set; }
        public Guid SLS_ID { get; set; }
        public string NumeroObligacionCompraCartera { get; set; }
    }
    public class ObtenerCausacionCorrienteDto
    {
        public long NumeroCredito { get; set; }
        public int TipoCredito { get; set; }
    }
    public class ObtenerCausacionCorrienteHistoricoDto
    {
        public long NumeroCredito { get; set; }
        public int TipoCredito { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
    }
}
