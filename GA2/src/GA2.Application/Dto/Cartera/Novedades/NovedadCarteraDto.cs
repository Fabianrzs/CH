using Microsoft.AspNetCore.Mvc;
using System;

namespace GA2.Application.Dto.Cartera.Novedades
{
    public class NovedadCarteraDto
    {

        public Guid IdNovedad { get; set; }
        public Guid IdCredito { get; set; }
        public int IdTipoNovedad { get; set; }
        public string TipoNovedad { get; set; }
        public DateTime FechaNovedad { get; set; }
        public DateTime FechaFinalizacion { get; set; }
        public int IdTipoCausal { get; set; }
        public string TipoCausal { get; set; }
        public int IdEstadoNovedad { get; set; }
        public string CodigoEstadoNovedad { get; set; }
        public string EstadoNovedad { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid? ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }


        // Valores calculados
        public decimal Capital { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal SeguroHogar { get; set; }
        public decimal InteresCorriente { get; set; }
        public decimal InteresMora { get; set; }
        public decimal CapitalRemanente { get; set; }
        public decimal InteresRemanente { get; set; }
        public decimal TotalDeuda { get; set; }
        public decimal? ValorPagado { get; set; }
        public decimal ValorRestante { get; set; }


        ////
        ///
        public int? CuotaCredito { get; set; }



        public FileResult? DataInforme { get; set; } = null;
        public DateTime? FechaProximoPago { get; set; }
        public decimal? ValorNuevaTasa { get; set; } = null;



        public decimal? PorcentajeSeguroHogar { get; set; } = 0;
        public decimal? PorcentajeSeguroVida { get; set; } = 0;
        public decimal? PorcentaeTasaNominalMensual { get; set; } = 0;
    }
}
