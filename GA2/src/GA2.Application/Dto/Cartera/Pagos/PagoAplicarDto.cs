using GA2.Application.Dto.Cartera.ProductoPorCliente;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GA2.Application.Dto
{
    public class PagoAplicarDto
    {
        public long NumeroCredito { get; set; }
        public decimal ValorPago { get; set; }
        public string Identificacion { get; set; }
        public DateTime Fecha_Pago_Cuota { get; set; }
        public DateTime Fecha_Pago { get; set; }
        public string Codigo { get; set; }
        public int nCuota { get; set; }
        public int TipoActor { get; set; } = 0;
        public int CuotasAcumuladas { get; set; } = 0;
        public Guid CreadoPor { get; set; }
        public int NumeroCuotas { get; set; } = 0;
        public bool AplicaSimulacion { get; set; } = false;
        public int Tcr_id { get; set; }
        public bool aplazarDisminucionCuota { get; set; }

        public decimal? ValorInteresRemanente { get; set; }
        public decimal? ValorCapitalRemanente { get; set; }
        public bool IncluyeMora { get; set; }

        // ✅ Nuevas propiedades para OTROS_PAGO
        public decimal? SeguroVidaPendiente { get; set; }
        public decimal? SeguroHogarPendiente { get; set; }
        public decimal? InteresMorePendiente { get; set; }
        public decimal? InteresCorrientePendiente { get; set; }
        public decimal? CapitalMorePendiente { get; set; }
        public decimal? CapitalPendiente { get; set; }
        public string? Observaciones { get; set; }
        public decimal? TotalPagadoMes { get; set; }
        public decimal? TotalPlanMes { get; set; }
        public string TipoRecurso { get; set; } = string.Empty;
    }
}
