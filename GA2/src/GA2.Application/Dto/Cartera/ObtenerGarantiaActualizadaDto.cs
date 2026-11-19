using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera
{
    public class ObtenerGarantiaActualizadaDto
    {
        public string Identificacion { get; set; }
        public DateTime FechaDesembolso { get; set; }
        public DateTime FechaFinalizaPagos { get; set; }
        public decimal ValorAjusteAnterior { get; set; }
        public decimal PorcentajeIVC { get; set; }
        public decimal ValorAjuste { get; set; }
        public decimal NuevoValorAjustado { get; set; }

    }
}
