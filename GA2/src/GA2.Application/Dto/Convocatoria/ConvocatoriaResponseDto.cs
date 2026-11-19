using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto
{
    public class ConvocatoriaResponseDto
    {
        public int IdConvocatoria { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public long NumeroBeneficiario { get; set; }
        public long NumeroResolucion { get; set; }
        public long NumeroConvocatoria { get; set; }
        public long NumeroDiariOficial { get; set; }
        public string NombreEstado { get; set; }
        public int Delete { get; set; }
        public DateTime FechaResolucion { get; set; }
    }
}
