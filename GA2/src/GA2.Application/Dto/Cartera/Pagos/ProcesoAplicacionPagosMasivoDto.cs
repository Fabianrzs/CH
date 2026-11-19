using GA2.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GA2.Application.Dto.Cartera.Pagos
{
    public class ProcesoAplicacionPagosMasivoDto
    {
        public int IdProceso { get; set; }
        public DateTime Fecha_Pago_Cuota { get; set; }
        public DateTime Fecha_Pago { get; set; }
        public string Periodo { get; set; }
        public string UnidadEjecutora { get; set; }
        public string TipoCredito { get; set; }
        public int UnidadEjecutoraId { get; set; }
        public int TipoCreditoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid CreadoPor { get; set; }
        public Guid ModificadoPor { get; set; }
        public string Estado { get; set; }
        public string Descripcion { get; set; }
        public string JsonPagos { get; set; }
        public IEnumerable<PagoListaDetalleDto> Pagos { get; set; } = new List<PagoListaDetalleDto>();

        public decimal Total => Pagos?.Sum(x => x.ValorCuota) ?? 0;
    }
}
