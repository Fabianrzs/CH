using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.VerificacionDerechos;
using GA2.Application.Dto.VerificacionDerechos.CuentasAVAC;
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
    public class ObtenerDatosCuentasAVAC : BaseAsyncEndpoint
                                                          .WithRequest<VerificacionDerechosByFechaDto>
                                                          .WithResponse<Response<IEnumerable<ObtenerCuentasAVACDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerDatosCuentasAVAC(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de las cuentas AV",
           Description = "Obtiene los datos de las cuentas AV",
           OperationId = "CuentasConceptos.ObtenerDatosCuentasAV",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerCuentasAVAC")]
        public async override Task<ActionResult<Response<IEnumerable<ObtenerCuentasAVACDto>>>> HandleAsync([FromQuery] VerificacionDerechosByFechaDto request, CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerDatosCuentasAVAC(request);
        }
    }
}
