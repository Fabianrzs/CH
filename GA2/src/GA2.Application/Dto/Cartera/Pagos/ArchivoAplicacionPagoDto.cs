using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class ArchivoAplicacionPagoTXTDto
    {
        public string Documento { get; set; }
        public string Funcionario { get; set; }
        public int ConceptoCod { get; set; }
        public decimal Valor { get; set; }

    }

    public class ArchivoAplicacionPagoExcelDto
    {
        [JsonProperty(PropertyName = "070 HIPOTECARIO")]
        public List<LeasingHipotecarioXlsx> Hipotecario { get; set; }

        [JsonProperty(PropertyName = "071 LEASING")]
        public List<LeasingHipotecarioXlsx> Leasing { get; set; }
    }

    public class LeasingHipotecarioXlsx
    {
        [JsonProperty(PropertyName = "Periodo")]
        public string Periodo { get; set; }
        [JsonProperty(PropertyName = "Concepto (Código de descuento)")]
        public string Concepto { get; set; }
        [JsonProperty(PropertyName = "Desprendible")]
        public int? Desprendible { get; set; }
        [JsonProperty(PropertyName = "Documento")]
        public string Documento { get; set; }
        [JsonProperty(PropertyName = "Valor Aplicado")]
        public string ValorAplicado { get; set; }
        [JsonProperty(PropertyName = "Cuotas Pendientes")]
        public string CuotasPendientes { get; set; }
        [JsonProperty(PropertyName = "Fecha de Ingreso")]
        public DateTime? FechaIngreso { get; set; }
    }

    public class ArchivoAplicacionPagoCSVDto
    {
        public string FuerzaNombre { get; set; }
        public string TipoNomina { get; set; }
        public string CodConcepto { get; set; }
        public string Concepto { get; set; }
        public string Funcionario { get; set; }
        public string NroDocumento { get; set; }
        public string CodigoMilitar { get; set; }
        public decimal ValorCuota { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}
