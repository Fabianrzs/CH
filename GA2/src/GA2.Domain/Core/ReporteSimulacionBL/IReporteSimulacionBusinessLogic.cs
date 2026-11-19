using GA2.Application.Dto.ReporteSimulacion;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReporteSimulacionBL
{
    public interface IReporteSimulacionBusinessLogic
    {
        Task<Response<bool>> GuardarReporteSimulaciones(RequestReporteSimulacionDto Reporte);
    }
}
