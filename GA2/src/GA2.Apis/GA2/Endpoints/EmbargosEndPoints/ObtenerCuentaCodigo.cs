using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Embargos;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    [Authorize]
    public class ObtenerCuentaCodigo : BaseAsyncEndpoint
        .WithRequest<DespachoDto>.WithResponse<Response<IEnumerable<JuzgadosDto>>>
    {

        IEmbargosBusinessLogic _embargosBusinessLogic;

        public ObtenerCuentaCodigo(IEmbargosBusinessLogic embargosBusinessLogic)
        {
            _embargosBusinessLogic = embargosBusinessLogic;
        }


        [HttpPost("api/Embargos/ObtenerCuentaCodigo")]
        [ProducesResponseType(typeof(Response<IEnumerable<JuzgadosDto>>), StatusCodes.Status200OK)]        
        [SwaggerOperation(
           Summary = "Obtener Despacho por Codigo y Numero de Cuenta",
           Description = "Obtener Despacho por Codigo y Numero de Cuenta",
           OperationId = "Embargos.ObtenerCuentaCodigo",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<IEnumerable<JuzgadosDto>>>> HandleAsync(DespachoDto request, CancellationToken cancellationToken = default)
        {
            return await this._embargosBusinessLogic.ObtenerJuzgadosPorNumeroCuentaCodigo(request);
        }
    }
}
