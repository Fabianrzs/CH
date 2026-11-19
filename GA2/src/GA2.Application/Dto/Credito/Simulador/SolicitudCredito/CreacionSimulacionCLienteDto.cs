using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GA2.Application.Dto
{
    public class CreacionSimulacionClienteDto
    {
        [Key]
        public string documento { get; set; }
        public string tipoVivienda { get; set; }
        public double? valor { get; set; }
        public double? ValorViviendaSeguro { get; set; }
        //public string tipoAbono { get; set; }
        //public string subsidio { get; set; }
        //public bool abonoUnico { get; set; }
        public int plazo { get; set; }
        public double valorPrestamo { get; set; }
        public double valorCuota { get; set; }
        public int simId { get; set; }
        public int tipoIdentificacion { get; set; }
        public decimal porOpcionCompra { get; set; }
        public decimal CanonInicial { get; set; }
        public bool ViviendaNueva { get; set; }
        public bool AbonoExtra { get; set; }
        public int TipoAbono { get; set; }
        public bool AbonoUnico { get; set; }
        public bool AplicaBeneficio { get; set; }
        public bool TasaVoluntariaAportes { get; set; }
        public decimal avaluoInmueble { get; set; }
        public decimal valorInicialSimuladoInmueble { get; set; }
        public int tipoSimularOProspectar { get; set; }
        public Guid? Usr_id { get; set; }
        public int afl_id { get; set; }
        public DateTime FechaDesembolso { get; set;  }
        public string NumeroObligacionSimulacion { get; set; }

    }
}
