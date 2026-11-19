using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Respuesta consolidada con Saldo Actual + Histórico
    /// </summary>
    public class RespuestaCausacionPorCredito
    {
        public long NumeroCredito { get; set; }
        public int IdTipoCredito { get; set; }
        public string NombreTipoCredito { get; set; }
        public DateTime FechaConsulta { get; set; }

        public CausacionSaldoActual SaldoActual { get; set; }
        public System.Collections.Generic.List<CausacionHistorico> Historico { get; set; }

        public RespuestaCausacionPorCredito()
        {
            Historico = new System.Collections.Generic.List<CausacionHistorico>();
            FechaConsulta = DateTime.Now;
        }
    }
}











