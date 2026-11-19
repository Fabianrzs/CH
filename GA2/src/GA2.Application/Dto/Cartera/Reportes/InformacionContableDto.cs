using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class InformacionContableDto
    {
        public DateTime FechaMovimiento { get; set; }
        public int? Comprobante { get; set; }
        public long TipoComprobante { get; set; }
        public string Detalle { get; set; }
        public string Movimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal? Saldo { get; set; }
        public string Concepto { get; set; }
        public string Tercero { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreTercero { get; set; }
        public string NumeroCuentaContable { get; set; }
        public string NombreCuentaContable { get; set; }
    }
}
