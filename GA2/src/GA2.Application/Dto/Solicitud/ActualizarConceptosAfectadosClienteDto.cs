using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Solicitud
{
    public class ActualizarConceptosAfectadosClienteDto
    {       
        public string entidadObj { get; set; }
        public string identificacion { get; set; }
        public string numeroCuenta { get; set; }
        public Guid solicitudId { get; set; }
        public Guid solicitudTareaId { get; set; }
        public int tipoIdentificacion { get; set; }
        public decimal valorPagar { get; set; }
        public string TipDoc { get; set;  }
        public string TipoCuenta { get; set; }
    }
}
