using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class CreditoFinancieroDto
    {
        public Guid IdSolicitudF { get; set; }
        public Guid IdSolicitudCredito { get; set; }
        public string ConceptoSalario { get; set; }
        public double ValorConceptoSalario { get; set; }
        public string Concepto1 { get; set; }
        public double ValorConcepto1 { get; set; }
        public string Concepto2 { get; set; }
        public double ValorConcepto2 { get; set; }
        public string Concepto3 { get; set; }
        public double ValorConcepto3 { get; set; }
        public string Concepto4 { get; set; }
        public double ValorConcepto4 { get; set; }
        public string Concepto5 { get; set; }
        public double ValorConcepto5 { get; set; }
        public string Concepto6 { get; set; }
        public double ValorConcepto6 { get; set; }
        public double ValorTotalIngresos { get; set; }
        public double ValorTotalEgresos { get; set; }
        public string DireccionVivienda { get; set; }
        public string MatriculaVivienda { get; set; }
        public double ValorComercialVivienda { get; set; }
        public string MarcaVehiculo { get; set; }
        public string PlacaVehiculo { get; set; }
        public double ValorComercialVehiculo { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ActualizadoPor { get; set; }
        public DateTime FechaActualiza { get; set; }
    }
}
