using System;

namespace GA2.Application.Dto
{
    public class ParamGeneralesCreditoYCarteraDto
    {
        public int parametroSalarioMinimo{ get; set; }
        public decimal porcentajeInflacion { get; set; }
        public decimal porcentajeViviendaVis { get; set; }
        public decimal porcentajeViviendaNoVis { get; set; }
        public decimal limiteSalariosVis { get; set; }
        public int diasPreaprobado { get; set; }
        public int diasAprobado { get; set; }
        public int puntajeMinimoCentrales { get; set; }
        public decimal porcentajeMinimoEndeudamiento { get; set; }
        public int vigenciaDiasConsultaCentrales { get; set; }
        public int porcentajeMinimoCanonInicial { get; set; }
        public int porcentajeMaximoCanonInicial { get; set; }
        public int porcentajeMaximoOpcionCompra { get; set; }
        public int maximoMesesAlivioTasa { get; set; }
        public int numeroMesesAlivio { get; set; }
        public decimal factorAhorro { get; set; }
        public decimal factorCesantias { get; set; }
        public decimal factorSubsidio { get; set; }
        public decimal porcentajeIncrementoAnual { get; set; }
        public int tarifaUnicaAvaluos { get; set; }
        public int tarifaEstudioTitulos { get; set; }
        public int valorMinimoCuota { get; set; }
        public decimal valorTasaUsura { get; set; }
        public DateTime FechaTasaUsura { get; set; }
        public decimal valorTasaMora { get; set; }
        public DateTime FechaTasaMora { get; set; }
        public bool ActiveTu { get; set; }
        public int Score_Min_TU { get; set; }
        public int valor_SMMLV { get; set; }
        public int MesesDiferir { get; set; }
        public bool activeBlob { get; set; }
        public decimal TasaUsuraPeriodica { get; set; }
        public decimal valorIpvAnual { get; set; }

        public decimal ValorIpvAnualOtrasCiudades { get; set; }
        public decimal PorcentajeEndeudamiento { get; set; }
        public bool AhorroIntereses { get; set; }
        public bool CesantiasIntereses { get; set; }
        public bool RangoAscenso { get; set; }
        public bool IncluirObligacionesExternasCapacidadPago { get; set; }
        public bool AsignacionSalarioCargo { get; set; }
    }
}
