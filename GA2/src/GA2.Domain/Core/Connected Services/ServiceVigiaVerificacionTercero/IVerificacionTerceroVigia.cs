using GA2.Transversals.Commons;
using ServiceVigiaVerificacionTercero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Connected_Services.ServiceVigiaVerificacionTercero
{
    public interface IVerificacionTerceroVigia
    {
        Task<Response<Resultado>> VertificacionTerceroSolicitud(string param1, string param2, string param3);
    }
}
