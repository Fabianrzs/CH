using System;
using System.Collections;

namespace GA2.Application.Dto
{
    public class ArchivoAplicacionPagoCSVComparacionDto
    {
        public string FuerzaNombre { get; set; }
        public string TipoNomina { get; set; }
        public string CodConcepto { get; set; }
        public string Concepto { get; set; }
        public string Funcionario { get; set; }
        public string NroDocumento { get; set; }
        public string CodigoMilitar { get; set; }
        public decimal ValorCuota { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string Observaciones { get; set; }
    }
}
