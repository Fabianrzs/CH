using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ProspeccionTransUnionRequestDto
    {
        public int TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string Telefono { get; set; }
        public string FechaExpedicionIdentificacion { get; set; }
        public string Apellidos { get; set; }
        public decimal SalarioMensual { get; set; }
        public string TraerHistorialCrediticio { get; set; }
        public Guid SLS_ID { get; set; }
    }
}
