using System;

namespace GA2.Domain.Entities.Cartera.Cierres.CierreAnual
{
    // ================================================================================
    // DTOs Y ENTIDADES PARA REESTRUCTURACIÓN
    // ================================================================================

    namespace Cartera.Entities
    {
        /// <summary>
        /// Entidad para histórico de reestructuraciones
        /// </summary>
        public class HistoricoReestructuracion
        {
            public int Id { get; set; }
            public long NumeroCredito { get; set; }
            public int TipoCredito { get; set; }
            public DateTime FechaReestructuracion { get; set; }
            public DateTime FechaSolicitud { get; set; }
            public int PlazoAnterior { get; set; }
            public int PlazoNuevo { get; set; }
            public double TasaAnterior { get; set; }
            public double TasaNueva { get; set; }
            public decimal ValorPagoRealizado { get; set; }
            public decimal ValorPagoMinimo { get; set; }
            public decimal InteresesCorrientes { get; set; }
            public decimal InteresesMora { get; set; }
            public decimal SaldoCapitalAnterior { get; set; }
            public int DiasMoraAlReestructurar { get; set; }
            public decimal CuotaAnterior { get; set; }
            public decimal CuotaNueva { get; set; }
            public string Observaciones { get; set; }
            public string CreadoPor { get; set; }
            public DateTime FechaCreacion { get; set; }
        }
    }
}

