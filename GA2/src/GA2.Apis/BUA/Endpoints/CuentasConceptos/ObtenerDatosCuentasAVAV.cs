using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAV;
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
    public class ObtenerDatosCuentasAVAV : BaseAsyncEndpoint
                                                          .WithRequest<VerificacionDerechosByFechaDto>
                                                          .WithResponse<Response<IEnumerable<ObtenerCuentasAVAVDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerDatosCuentasAVAV(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de las cuentas AV",
           Description = "Obtiene los datos de las cuentas AV",
           OperationId = "CuentasConceptos.ObtenerDatosCuentasAV",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerCuentasAVAV")]
        public async override Task<ActionResult<Response<IEnumerable<ObtenerCuentasAVAVDto>>>> HandleAsync([FromQuery] VerificacionDerechosByFechaDto request, CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerDatosCuentasAVAV(request);
        }
    }
}
