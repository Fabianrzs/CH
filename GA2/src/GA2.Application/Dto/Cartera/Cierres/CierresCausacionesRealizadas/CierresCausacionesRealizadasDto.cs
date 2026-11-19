using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.Cartera.Cierres.CierresCausacionesRealizadas
{

    public class CierresCausacionesRealizadasDto
    {
        public int IdCierre { get; set; }
        public string TipoCierre { get; set; }
        public DateTime FechaCierre { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public Guid UsuarioEjecutaCierre { get; set; }
        public int IdTipoCierre { get; set; }
        public string NombreTipoCierre { get; set; }
        public string DescripcionTipoCierre { get; set; }
    }
}
