using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.ProductoPorCliente
{
    public class InsertProductoClienteDto
    {
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public decimal ValorDesembolso { get; set; }
        public decimal ValorCuota { get; set; }
        public int PlazoTotal { get; set; }
        public int TipoCreditoModalidad { get; set; }
        public decimal ValorViviendaSeguroHogar { get; set; }
        public decimal valorInmueblePlanPagos { get; set; }
        public int ViviendaVis { get; set; }
        public int EstadoVivienda { get; set; }
        public decimal SaldoCapital { get; set; }
        public decimal TasaSeguroHogar { get; set; }
        public decimal TasaSeguroVida { get; set; }
        public decimal TasaEA { get; set; }
        public decimal TasaPER { get; set; }
        public decimal TasaFrech { get; set; }
        public decimal ValorAlivioCuota { get; set; }
        public string TipoAbonoExtra { get; set; } 
        public decimal ValorAbonoExtra { get; set; }
        public decimal PorCanonInicial { get; set; }
        public decimal PorOpcionCompra { get; set; }
        public bool AlivioFrech { get; set; }
        public Guid CreadoPor { get; set; }
        public long NumeroCredito { get; set; }
        public string NumeroIdentificacion { get; set; }
        public int TipoIdentificacion { get; set; }
        public string DireccionInmueble { get; set; }
        public int CiudadInmueble { get; set; }
        public string NumeroMatricula { get; set; }
        public string EscrituraInmueble { get; set; }
        public DateTime FechaEscritura { get; set; }
        public string Notaria { get; set; }
        public string Linderos { get; set; }
        public string DescripcionInmueble { get; set; }
        public int Score { get; set; }
        public string Calificacion { get; set; }
        public int ClaseInmueble { get; set; }
        public decimal SaldoCapitalTradicional { get; set; }
        public decimal SaldoCuotaTradicional { get; set; }
        public int LineaCredito { get; set; }
        public decimal RecursosFuturos { get; set; }
        public IFormFile ArchivoPlanPagos { get; set; }
        public decimal PorcentajeSeguroVida { get; set; }
        public decimal PorcentajeExtraPrimaSeguroVida { get; set; }
        public string AseguradoraVida { get; set; }
        public List<PlanPagoDto> PlanPago { get; set; }
    }
}
