using ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.Connected_Services.ServiceReference1
{
    public interface IBuaService2BusinessLogic
    {
        Task<Transversals.Commons.Response<BUA>> ObtenerBUAAsync(string document, int typeDocumento);
    }
}
