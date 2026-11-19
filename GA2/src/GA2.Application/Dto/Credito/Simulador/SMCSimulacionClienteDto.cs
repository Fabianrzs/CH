using System;
using System.Collections.Generic;
using System.Text;

namespace GA2.Application.Dto
{
    public class SMCSimulacionClienteDto
    {
        public Guid IdSimulacionCliente { get; set; }
        public Guid IdSimulacion { get; set; }
        public Guid IdDatosPersonales { get; set; }
        public string TipoVivienda { get; set; }
        public double? ValorVivienda { get; set; }
        public double? ValorViviendaSeguro { get; set; }
        public string TipoAlivio { get; set; }
        public string TipoAbono { get; set; }
        public string Documento { get; set; }
        public decimal ValorPrestamo { get; set; }
        public decimal TasaEa { get; set; }
        public double TasaMv { get; set; }
        public double TasaFrech { get; set; }
        public string ViviendaVis { get; set; }
        public DateTime FechaSimulacion { get; set; }
        public string UsaRecursosAcumulados { get; set; }
        public string TomaSeguro { get; set; }
        public double ValorPrestamosLeasing { get; set; }
        public int NumeroPreaprobado { get; set; }
        public int TotalCuotas { get; set; }
        public DateTime FechaAprobacion { get; set; }
        public double OpcionCompra { get; set; }
        public double CuotaConSeguros { get; set; }
        public double CuotaSinSeguros { get; set; }
        public int TcrId { get; set; }
        public double alivioTasaporcentaje { get; set; }
        public double TasaSeguroVida { get; set; }
        public double TasaSeguroHogar { get; set; }
        public double ValorPrestamoSolicitado { get; set; }
        public decimal PorcentajeCanonInicial { get; set; }
        public decimal PorcentajeOpcionCompra { get; set; }
        public int AportesVoluntarios { get; set; }
        public decimal factor { get; set; }
        public Guid? Usr_id { get; set; }
        public int afl_id { get; set; }
        public int NumeroFolios { get; set; }
        public Guid MediosCredito { get; set; }
        public int MedioRecepcion { get; set; }
        public int CLC_ID { get; set; }
    }
}
