using System;

namespace GA2.Application.Dto.Cartera.Reportes
{
    public class CarteraReporteGeneralDto
    {
        public Guid CreditoId { get; set; }
        public long NumeroCredito { get; set; }
        public string Identificacion { get; set; }
        public string NombreCompleto { get; set; }
        public int IdTipoCredito { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public int IdFuerza { get; set; }
        public int IdGrado { get; set; }
        public int IdCategoria { get; set; }
        public decimal ValorDesembolso { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public decimal ValorCuota { get; set; }
        public decimal TasaCredito { get; set; }
        public DateTime FechaUltimoPago { get; set; }
        public DateTime FechaProximoPago { get; set; }
        public DateTime FechaFinalizaPagos { get; set; }
        public int PlazoTotal { get; set; }
        public int Cre_Cartera_Castigada { get; set; }
        public int NumeroCuotasCanceladas { get; set; }
        public int NumeroCuotasFaltantes { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal ValorVivienda { get; set; }
        public decimal ValorOpcionCompra { get; set; }
        public decimal ValorDeudaRemanente { get; set; }
        public string CiudadInmueble { get; set; }
        public string TipoVivienda { get; set; }
        public int IdLineaCredito { get; set; }
        public string Calificacion { get; set; }
        public int Puntaje { get; set; }
        public string Unidadejecutora { get; set; }
        public string Grado { get; set; }
        public string Fuerza { get; set; }
        public string Categoria { get; set; }
        public string TipoCredito { get; set; }
        public string LineaCredito { get; set; }
        public decimal ValorIntereses { get; set; }
        public decimal ValorSeguros { get; set; }
        public decimal ValorCapital { get; set; }
        public decimal ValorTotalPagar { get; set; }
        public decimal ValorMoraAnterior { get; set; }
        public int DiasMora { get; set; }
        public string Estado_Descripcion { get; set; }
        public int Estado_Id { get; set; }
        public string DescripcionCiudad { get; set; }
        public decimal CanonExtraordinarioFinal { get; set; }
        public decimal ProvisionTotal { get; set; }



    }
}
