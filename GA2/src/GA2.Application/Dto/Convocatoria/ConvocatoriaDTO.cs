using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ConvocatoriaDTO
    {
        [Key]
        public int IdConvocatoria { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public long NumeroBeneficiario { get; set; }
        public long NumeroResolucion { get; set; }
        public long NumeroConvocatoria { get; set; }
        public long NumeroDiariOficial { get; set; }
        public int EstadoConvocatoriaId { get; set; }
        public DateTime FechaResolucion { get; set; }


    }
}
