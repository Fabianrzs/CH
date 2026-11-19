using System;

namespace GA2.Application.Dto
{
    public class AplicacionPagoDto
    {
        public Guid IdAplicacionPago { get; set; }
        public Guid IdCredito { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public decimal AbonoSeguroHogar { get; set; }
        public decimal AbonoSeguroVida { get; set; }
        public decimal AbonoInteresCorriente { get; set; }
        public decimal AbonoInteresMora { get; set; }
        public decimal AbonoCapital { get; set; }
        public decimal AbonoCapitalMora { get; set; }
        public decimal ValorPago { get; set; }
        public int IdDct { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificacion { get; set; }
        public string TipoPago { get; set; }
        public decimal CapitalRemanente { get; set; }
        public decimal InteresRemanente { get; set; }
        public decimal TotalDeudaRemanente { get; set; }
        public decimal ValorCanonExtraordinario { get; set; }
        public string ModalidadPago { get; set; }
        public decimal RemanenteUltimoPago { get; set; }
        public Guid IdPagoReversado { get; set; }
        public bool EsPagoReversible { get; set; }
        public string ComprobanteInterno { get; set; }
        public int ComprobanteSeven { get; set; }
        public int EstadoComprobante { get; set; }
        public bool IncluyeMora { get; set; }
        public string TipoRecurso { get; set; }
    }

    public class ParametrosAplicacionPagoDto
    {
        public DateTime FechaAplicacion { get; set; }
        public decimal ValorPago { get; set; }
        public int FormatoAplicar { get; set; }
        public int nCuota { get; set; }
        public bool AplicanPagos { get; set; }
        public Guid CreadoPor { get; set; }
        public bool AplicaSimulacion { get; set; }
    }

}
