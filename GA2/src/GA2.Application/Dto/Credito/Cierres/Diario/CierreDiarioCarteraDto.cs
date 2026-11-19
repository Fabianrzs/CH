using System;

namespace GA2.Application.Dto
{

    public class CierreDiarioCarteraDto
    {
        public long NumeroCredito { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Nombres { get; set; }
        public string UnidadEjecutora { get; set; }
        public string Categoria { get; set; }
        public string Grado { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string TipoCredito { get; set; }
        public string EstadoCredito { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaFinalizaPagos { get; set; }
        public decimal ValorDesembolso { get; set; }
        public decimal ValorVivienda { get; set; }
        public string TipoVivienda { get; set; }
        public string ClasificacionVivienda { get; set; }
        public decimal CanonInicial { get; set; }
        public decimal ValorOpcionCompra { get; set; }
        public decimal PorcentajeCanonInicial { get; set; }
        public decimal PorcentajeOpcionCompra { get; set; }
        public decimal CanonExtraordinario { get; set; }
        public int PlazoTotal { get; set; }
        public int PlazoActual { get; set; }
        public int CuotasCanceladas { get; set; }
        public decimal TasaSeguroVida { get; set; }
        public decimal TasaSeguroHogar { get; set; }
        public decimal TasaCreditoEA { get; set; }
        public decimal TasaPeriodica { get; set; }
        public decimal ValorCuota { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal SaldoMora { get; set; }
        public int DiasMora { get; set; }
        public decimal ValorDeudaRemanente { get; set; }
        public DateTime FechaEstado { get; set; }
        public DateTime FechaUltimoPago { get; set; }
        public DateTime FechaProximoPago { get; set; }
        public decimal AcumuladoSeguroHogar { get; set; }
        public DateTime FechaUltimoPagoSeguroHogar { get; set; }
        public decimal AcumuladoSeguroVida { get; set; }
        public DateTime FechaUltimoPagoSeguroVida { get; set; }
        public decimal AcumuladoInteresesMora { get; set; }
        public DateTime FechaUltimoPagoInteresesMora { get; set; }
        public decimal AcumuladoInteresesCorrientes { get; set; }
        public DateTime FechaUltimoPagoInteresesCorrientes { get; set; }
        public string Calificacion { get; set; }
        public decimal Provision { get; set; }
        public decimal ProvisionPorcentaje { get; set; }
        public decimal ProvisionAdicional { get; set; }
        public decimal CausacionInteresesCorriente { get; set; }
        public decimal CausacionInteresesMora { get; set; }

        public decimal TasaCreditoEAMora { get; set; }
        public decimal TasaPeriodicaMora { get; set; }
        public int AreaNegocio { get; set; }
        public int ComprobanteInternoLeasing { get; set; }
        public int ComprobanteInternoHipotecario { get; set; }
        public decimal IntMoraAnterior { get; set; }
        public decimal TotalMora { get; set; }
        public int DiasAcumuladosMora { get; set; }
        public decimal CausacionDeHoy { get; set; }
    }
}