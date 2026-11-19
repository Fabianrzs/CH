using Ardalis.ApiEndpoints;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVC;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class ObtenerInfoCuentaByPeriodo : BaseAsyncEndpoint
                                                          .WithRequest<ObtenerInfoCedulaRequestDto>
                                                          .WithResponse<Response<DatosInfoCensatiaByCuentaPeriodoDto>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerInfoCuentaByPeriodo(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de cedula de las cuentas Cesantias",
           Description = "Obtiene los datos de cedula de la cuentas Cesantias by periododo",
           OperationId = "CuentasConceptos.ObtenerInfoCuentaByPeriodo",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerInfoCuentaByPeriodo")]
        public async override Task<ActionResult<Response<DatosInfoCensatiaByCuentaPeriodoDto>>> HandleAsync([FromQuery] ObtenerInfoCedulaRequestDto request,CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerDatosCesantiasByCuentaId(request);
        }
    }
}
