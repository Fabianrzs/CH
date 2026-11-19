using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    [Authorize]
    public class ObtenerCuentaConceptoIdCuenta : BaseAsyncEndpoint.WithRequest<DocumentoIdCuenta>.WithResponse<Response<IEnumerable<CuentaPorConceptoDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public ObtenerCuentaConceptoIdCuenta(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }


        [SwaggerOperation(
        Summary = "Obtiene los datos basicos de los conceptos de la cuenta",
           Description = "Obtiene los datos basicos de conceptos de cuentas",
           OperationId = "CuentasConceptos.ObtenerCuentaConceptoIdCuenta",
           Tags = new[] { "CuentasConceptosEndPoints" })]
        [HttpGet("api/ObtenerCuentaConceptoIdCuenta")]
        public override async Task<ActionResult<Response<IEnumerable<CuentaPorConceptoDto>>>> HandleAsync([FromQuery] DocumentoIdCuenta request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.ObtenerCuentaConceptoIdCuenta(request.idCuenta);
        }
    }
}
