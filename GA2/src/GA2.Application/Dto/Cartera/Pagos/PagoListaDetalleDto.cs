using System;

namespace GA2.Application.Dto.Cartera.Pagos
{
    public class PagoListaDetalleDto
    {
        public int IdProceso { get; set; }
        public long IdDetalle { get; set; }
        public string FuerzaNombre { get; set; }
        public string TipoNomina { get; set; }
        public string CodConcepto { get; set; }
        public string Concepto { get; set; }
        public string Funcionario { get; set; }
        public string NroDocumento { get; set; }
        public string CodigoMilitar { get; set; }
        public Guid IdCredito { get; set; }
        public Guid CpgId { get; set; }
        public long NumeroCredito { get; set; }
        public decimal ValorSeguroHogar { get; set; }
        public decimal ValorSeguroVida { get; set; }
        public decimal ValorInteresCorriente { get; set; }
        public decimal ValorCapital { get; set; }
        public decimal ValorCuota { get; set; }
        public decimal ValorInteresRemanente { get; set; }
        public decimal ValorCapitalRemanente { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
