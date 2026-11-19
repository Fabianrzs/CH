using System;

namespace GA2.Application.Dto.Cartera.Cierres.CierreAnual
{
    public class ReporteCierreAnualCarteraDto
    {
        public Guid IdCredito { get; set; }
        public long NumeroCredito { get; set; }
        public string Identificacion { get; set; }
        public string TipoIdentificacion { get; set; }
        public string NombreCompleto { get; set; }
        public int IdTipoCredito { get; set; }
        public string TipoCredito { get; set; }
        public string UnidadEjecutora { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal SaldoCapitalTradicional { get; set; }
        public decimal AcumuladoSeguroHogar { get; set; }
        public decimal AcumuladoSeguroVida { get; set; }
        public decimal ProvisionGenerica { get; set; }
        public decimal AbonoSeguroHogar { get; set; }
        public decimal AbonoSeguroVida { get; set; }
        public decimal AbonoInteresCorriente { get; set; }
        public decimal AbonoInteresMora { get; set; }
        public decimal AbonoCapital { get; set; }
        public decimal AbonoCapitalRemanente{ get; set; }
        public decimal AbonoInteresRemanente { get; set; }
        public DateTime FechaPrimerPago { get; set; }
        public DateTime FechaPrimeraAplicacion { get; set; }
        public DateTime FechaUltimoPago { get; set; }
        public DateTime FechaUltimaAplicacion { get; set; }
        public int Anio { get; set; }
        public int DiasMora { get; set; }
        public string CalificacionCartera { get; set; }
        public int IdEstadoCredito { get; set; }
        public decimal Provision { get; set; }
        public decimal ProvisionProcentaje { get; set; }
        public decimal ProvisionAdicional { get; set; }
    }
}
