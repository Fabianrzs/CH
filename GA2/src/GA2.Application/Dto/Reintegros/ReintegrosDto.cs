using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ReintegrosDto
    {
        public int IdReintegro { get; set; }
        public string NumeroRadicado { get; set; }
        public int IdUnidadEjecutora { get; set; }
        public int IdTipoReintegro { get; set; }
        public int IdEstadoReintegro { get; set; }
        public DateTime FechaReintegro { get; set; }
        public decimal ValorReintegro { get; set; }
        public Guid CreadoPor { get; set; }
        public DateTime FechaCreacion { get; set; }
        public Guid ModificadoPor { get; set; }
        public DateTime FechaModificado { get; set; }
        public DetalleReintegroDto DetalleReintegroDto { get; set; }

    }

}
