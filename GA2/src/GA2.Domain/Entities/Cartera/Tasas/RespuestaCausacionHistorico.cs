using System;

namespace GA2.Domain.Entities
{
    /// <summary>
    /// Respuesta de Histórico de Causación filtrado por período de fechas
    /// </summary>
    public class RespuestaCausacionHistorico
    {
        public long NumeroCredito { get; set; }
        public int IdTipoCredito { get; set; }
        public string NombreTipoCredito { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public int TotalDias { get; set; }
        public int TotalRegistros { get; set; }
        public System.Collections.Generic.List<CausacionHistorico> Historico { get; set; }
        public DateTime FechaConsulta { get; set; }

        public RespuestaCausacionHistorico()
        {
            Historico = new System.Collections.Generic.List<CausacionHistorico>();
            FechaConsulta = DateTime.Now;
        }
    }
}











