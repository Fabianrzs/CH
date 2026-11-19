using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Application.Dto.PuntosAtencion
{
    public class PuntosAtencionDto
    {
        public int Id_PuntoAtencion { get; set; }
        public string PuntoAtencion { get; set; }
        public int TipoAfiliadoPunto { get; set; }
    }
}
