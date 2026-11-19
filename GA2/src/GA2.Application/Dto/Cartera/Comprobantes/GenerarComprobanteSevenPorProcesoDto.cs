using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Comprobantes
{
    public class GenerarComprobanteSevenPorProcesoDto
    {
        [JsonIgnore]
        public Guid CreadoPor { get; set; }
        public string CodigoProceso { get; set; }
        public List<ComprobanteDto> Comprobantes { get; set; }
    }
    public class ComprobanteDto
    {
        public int Comprobante_Id { get; set; }
    }
}
