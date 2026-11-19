using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class InclusionSaldoRequestDto
    {
        public int IdInclusionSaldoEncabezado { get; set; }
        public int IdCliente { get; set; }
        public int IdCuenta { get; set; }
        public List<InclusionSaldoDto> Opciones { get; set; }   
        public string Observaciones { get; set; }
        public bool GeneraOrdenRecaudo { get; set; }
        public DateTime FechaInicialRecaudo { get; set; }
        public DateTime FechaFinalRecaudo { get; set; }
        public string NumeroReferencia { get; set; }
        public bool Aprobacion { get; set; }
    }
}
