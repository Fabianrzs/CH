using GA2.Domain.Core;
using GA2.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.BUA.Endpoints.CuentasConceptos
{
    public class LogMovimientoConceptoCuentas: BaseAsyncEndpoint
        .WithRequest<List<LogMovimientoDto>>
        .WithResponse<Response<List<ConceptoCuentaDto>>>
    {
        private readonly ICuentasConceptosBusinessLogic _cuentasConceptosBusinessLogic;

        public LogMovimientoConceptoCuentas(ICuentasConceptosBusinessLogic cuentasConceptosBusinessLogic)
        {
            _cuentasConceptosBusinessLogic = cuentasConceptosBusinessLogic;
        }

        [HttpPost("api/CrearLogMovimientoCuenta")]
        [ProducesResponseType(typeof(Response<List<ConceptoCuentaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta log movimiento concepto cuenta ",
           Description = "Inserta log movimiento concepto cuenta",
           OperationId = "CuentasConceptos.CrearLogMovimientoCuenta",
           Tags = new[] { "CrearLogMovimientoCuenta" })]
        public override async Task<ActionResult<Response<List<ConceptoCuentaDto>>>> HandleAsync(List<LogMovimientoDto> request, CancellationToken cancellationToken = default)
        {
            return await _cuentasConceptosBusinessLogic.CrearLogMovimientoCuenta(request);
        }
    }
}
