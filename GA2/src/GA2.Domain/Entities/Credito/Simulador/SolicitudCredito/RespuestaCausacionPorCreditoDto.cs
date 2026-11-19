using System;

namespace GA2.Domain.Entities.Credito.Simulador.SolicitudCredito
{
    /// <summary>
    /// Respuesta consolidada con Saldo Actual + Histórico
    /// </summary>
    public class RespuestaCausacionPorCreditoDto
    {
        public long NumeroCredito { get; set; }
        public int IdTipoCredito { get; set; }
        public string NombreTipoCredito { get; set; }
        public DateTime FechaConsulta { get; set; }

        public CausacionSaldoActualDto SaldoActual { get; set; }
        public System.Collections.Generic.List<CausacionHistoricoDto> Historico { get; set; }

        public RespuestaCausacionPorCreditoDto()
        {
            Historico = new System.Collections.Generic.List<CausacionHistoricoDto>();
            FechaConsulta = DateTime.Now;
        }
    }
}
