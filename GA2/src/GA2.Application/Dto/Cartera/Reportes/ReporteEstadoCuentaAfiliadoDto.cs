using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteEstadoCuentaAfiliadoDto
    {
        public Guid IdCredito { get; set; }
        public long NumeroCredito { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombreAfiliado { get; set; }
        public int IdTipoCredito { get; set; }
        public string TipoCredito { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public string UnidadEjecutora { get; set; }
        public int IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public int IdLineaCredito { get; set; }
        public string LineaCredito { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public decimal ValorDesembolso { get; set; }
        public decimal ValorVivienda { get; set; }
        public decimal ValorOpcionCompra { get; set; }
        public decimal ValorCanonInicial { get; set; }
        public decimal TasaCreditoEA { get; set; }
        public decimal TasaCreditoNOM { get; set; }
        public decimal Saldo { get; set; }
        public int PlazoTotal { get; set; }
        public decimal ValorCuota { get; set; }
        public DateTime FechaUltimoPago { get; set; }
        public DateTime FechaProximoPago { get; set; }
        public string Modalidad { get; set; }
        public string Moneda { get; set; }
        public string FormatoPlazo { get; set; }
        public string FormatoPago { get; set; }

    }
}
