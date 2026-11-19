using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Credito.Simulador.Comite
{
    public class CrearIntegranteComiteCreditoDto
    {
        [Key]
        public int idIntegranteComite { get; set; }
        public string nombre { get; set; }
        public string cargo { get; set; }
        public string tipoIntegrante { get; set; }
        public int estado { get; set; }
        public Guid creadoPor { get; set; }
    }
}
