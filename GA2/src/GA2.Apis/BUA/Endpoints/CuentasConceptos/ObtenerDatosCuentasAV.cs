using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Domain.Core;
using GA2.Domain.Entities.VerificacionDerechos;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    [Authorize]
    public class ObtenerDatosCuentasAV : BaseAsyncEndpoint
                                                          .WithRequest<VerificacionDerechosByFechaDto>
                                                          .WithResponse<Response<IEnumerable<DatosCuentasAVDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerDatosCuentasAV(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de las cuentas AV",
           Description = "Obtiene los datos de las cuentas AV",
           OperationId = "CuentasConceptos.ObtenerDatosCuentasAV",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerDatosCuentasAV")]
        public async override Task<ActionResult<Response<IEnumerable<DatosCuentasAVDto>>>> HandleAsync([FromQuery] VerificacionDerechosByFechaDto request, CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerDatosCuentasAV(request);
        }
    }
}
