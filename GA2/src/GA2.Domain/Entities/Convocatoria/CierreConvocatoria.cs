using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Entities
{
    public class CierreConvocatoria
    {
        public int IdConvocatoria { get; set; }
        public string NumeroResolucionCierre { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
        public int NumeroBeneficiarios { get; set; }
        public int NumeroConvocatoria { get; set; }
        public decimal ValorOficial { get; set; }
        public decimal ValorSubOficial { get; set; }
        public decimal ValorSoldado { get; set; }
        public string Acuerdo { get; set; }
        public DateTime FechaPublicacionDiario { get; set; }
        public DateTime FechaNotificacion { get; set; }
    }
}
