using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.VerificacionDerechos
{
    public class SolicitudConsecutivoDto
    {        
        public string CnsId { get; set; }
        public string CnsDescripcion { get; set; }
        public long CnsUltimoNumero { get; set; }
        public int CnsEstado { get; set; }
        public Guid CnsCreadoPor { get; set; }
        public DateTime CnsFechaCreacion { get; set; }
        public Guid CnsActualizadoPor { get; set; }
        public DateTime CnsFechaActualiza { get; set; }

    }

       

}
