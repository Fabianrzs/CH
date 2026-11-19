using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class ConceptoCuentaDto
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }
        public int ConceptoId { get; set; }
        public string ConceptoDescripcion { get; set; }
        public double saldo { get; set; }
        public DateTime FechaUltimoAporte { get; set; }
        public int Ingresos { get; set; }
        public double Sva { get; set; }
        public string DescripcionConcepto { get; set; }
        public decimal ValorEmbargo { get; set; }
        public decimal ValorBloqueoEmbargo { get; set; }
        public decimal ValorDevolver { get; set; }
        public double ValorNewSaldo { get; set; }
        public List<MovimientosCuentaDto> Movimientos { get; set; }

    }

}
