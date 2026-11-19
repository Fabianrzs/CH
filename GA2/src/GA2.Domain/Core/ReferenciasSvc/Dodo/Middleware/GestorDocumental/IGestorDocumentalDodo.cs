using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core.Dodo.Common;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GA2.Domain.Core.ReferenciasSvc.Dodo.Middleware.GestorDocumental
{
    interface IGestorDocumentalDodo
    {
        Task<Response<DodoFileOperationResponse>> RadicarFlujo361(SolicitudSimulacionDto preAprobado);
        Task<Response<DodoFileOperationResponse>> RadicarFlujo83(DatosIdentificacionDto datosIdentificacion);

    }
}
