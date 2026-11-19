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

namespace GA2.Apis.GA2.Endpoints.CuentasEndPoints
{
    [Authorize]
    public class ObtenerConceptoCuentaId : BaseAsyncEndpoint.WithRequest<CuentaRequest>.WithResponse<Response<IEnumerable<CuentaPorConceptoDto>>>
    {
        private readonly ICuentasClientesBusinessLogic _cuentasClientesBusinessLogic;

        public ObtenerConceptoCuentaId(ICuentasClientesBusinessLogic cuentasClientesBusinessLogic)
        {
            _cuentasClientesBusinessLogic = cuentasClientesBusinessLogic;
        }


        [SwaggerOperation(
           Summary = "Obtiene los datos basicos de los conceptos de la cuenta",
           Description = "Obtiene los datos basicos de conceptos de cuentas",
           OperationId = "CuentasConceptos.ObtenerConceptoCuentaId",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerConceptoCuentaId")]
        public override async Task<ActionResult<Response<IEnumerable<CuentaPorConceptoDto>>>> HandleAsync([FromQuery]  CuentaRequest request, CancellationToken cancellationToken = default)
        {
            return await _cuentasClientesBusinessLogic.ObtenerConceptoDeCuentas(request.idCuenta);
        }
    }
}
