using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteContableDto
    {
        public int TipoOperacion { get; set; }
        public int IdTipoCredito { get; set; }
        public string DescripcionOperacion { get; set; }
        public string DescripcionCuenta { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public long NumeroCredito { get; set; }
        public string CuentaSeven { get; set; }
        public string CuentaNif { get; set; }
        public decimal Debito { get; set; }
        public decimal Credito { get; set; }
        public long TotalRegistros { get; set; }
        public string CentroCostos { get; set; }
        public string CentroCostosCredito { get; set; }
        public string ComprobanteInterno { get; set; }

    }
}
