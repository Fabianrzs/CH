using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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
    public class ObtenerDatosVerificacionDerechosCuentas : BaseAsyncEndpoint
                                                          .WithRequest<VerificacionDerechosByFechaDto>
                                                          .WithResponse<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerDatosVerificacionDerechosCuentas(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos de las cuentas para las verificacion de derechos",
           Description = "Obtiene los datos de las cuentas para las verificacion de derechos",
           OperationId = "CuentasConceptos.ObtenerDatosVerificacionDerechosCuentas",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerDatosVerificacionDerechosCuentas")]
        public async override Task<ActionResult<Response<IEnumerable<DatosVerificacionDerechoCuentaDto>>>> HandleAsync([FromQuery] VerificacionDerechosByFechaDto request, CancellationToken cancellationToken = default)
        {

            return await _cuentasConceptosBusinessLogic.ObtenerDatosVerificacionDerechosCuentas(request);
        }
    }
}
