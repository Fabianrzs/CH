using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Constructora;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Constuctora
{
    public class ConsultarConstructora : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<ConstructoraDto>>>
    {
        private readonly IConstructoraBussines _constructora;

        public ConsultarConstructora(IConstructoraBussines constructoraBussigneLogic)
        {
            _constructora = constructoraBussigneLogic;
        }
        [HttpGet("api/Constructora/ConsultarConstructora")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConstructoraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar las constructora",
            Description = "Consultar las constructora",
            OperationId = "Constructora.ConsultarConstructora",
            Tags = new[] { "ConstructoraEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ConstructoraDto>>>> HandleAsync( CancellationToken cancellationToken = default)
        {
            return await _constructora.ConsultarConstructora();
        }
    }
}
