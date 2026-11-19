using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class ReporteAPNRCARDto
    {
        public long ConsecutivoNumeroDeContrato { get; set; }
        public int CreditoInversionInterbancario { get; set; }
        public int ClaseDeCreditoOInversion { get; set; }
        public int ClasificacionYPonderacion { get; set; }
        public string Nit { get; set; }
        public string RiesgoCortoPlazo { get; set; }
        public string RiesgoLargoPlazo { get; set; }
        public string InversionSubordinada { get; set; }
        public int DiasResidual { get; set; }
        public string Efectivo { get; set; }
        public int GarantizadosPorOEmitidoPor { get; set; }
        public int DestinoDelCredito { get; set; }
        public string Desembolso { get; set; }
        public string DestinoDeOtorgamiento { get; set; }
        public string Revocable { get; set; }
        public int ProcesosDeTitularizacion { get; set; }
        public decimal Provision { get; set; }
        public decimal CoberturaDeLaGarantiaValoracion { get; set; }
        public int ClaseDeGarantia { get; set; }
        public string GarantiaVivienda { get; set; }
        public int GarantiaViviendaInicial { get; set; }
        public int DiasDeIncumplimiento { get; set; }
        public int TipoDeCartera { get; set; }
        public string MonedaLocal { get; set; }
        public string ActivoEnIncumplimiento { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public DateTime FechaInicial { get; set; }
        public int PlazoInicial { get; set; }
        public DateTime FechaCorte { get; set; }
        public string CodigoGarantia { get; set; }
        public decimal ValorDeLaGarantiaDelCredito { get; set; }
        public decimal SaldoCredito { get; set; }
        public decimal SaldoCuentaPorCobrar { get; set; }
        public decimal PorcentajeDeGarantia { get; set; }
        public Guid IdCredito { get; set; }

    }
}
