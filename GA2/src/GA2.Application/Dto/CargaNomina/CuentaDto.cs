using System;
using System.Collections.Generic;

namespace GA2.Application.Dto
{
    public class CuentaDto
    {
        public int IdCuenta { get; set; }
        public int IdCuentaIntegracion { get; set; }
        public int NumeroCuenta { get; set; }
        public string NumeroCuentaCompleta { get; set; }
        public int Subcuenta { get; set; }
        public int IdTct { get; set; }
        public string TipoCuenta { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdEct { get; set; }
        public string? EstadoCuenta { get; set; }
        public string? EstadoCliente { get; set; }
        public int IdCcn { get; set; }
        public int IdDocumento { get; set; }
        public int IdTpa { get; set; }
        public int Cuotas { get; set; }
        public DateTime FechaCierre { get; set; }
        public int IdCuentaHabiente { get; set; }
        public int CuotasPendientes { get; set; }
        public DateTime FechaPrimerAporte { get; set; }
        public decimal SaldoCanje { get; set; }
        public decimal SaldoDisponible { get; set; }
        public decimal SaldoDisponibleEmbargos { get; set; }
        public decimal TotalEmbargos { get; set; }
        public decimal TotalBloqueos { get; set; }
        public decimal TotalReintegros { get; set; }
        public int Principal { get; set; }
        public int IdCliente { get; set; }
        public int conceptoId { get; set; }
        public string? CausalEstado { get; set; }
        public List<MovimientoDTO> Movimientos { get; set; }
        public List<ConceptosSaldosBloqueosDto> ConceptosSaldosBloqueos { get; set; }
        public List<ConceptosSaldosBloqueosDto> ConceptosEmbargosBloqueos { get; set; }
        public List<EmbargoCuentaConceptoDto> Embargos { get; set; }
        public List<ConceptoCuentaDto> Conceptos { get; set; }
    }
}