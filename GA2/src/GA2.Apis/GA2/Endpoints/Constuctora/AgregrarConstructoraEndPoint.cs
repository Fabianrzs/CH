using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Constructora;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Constuctora
{
    public class AgregrarConstructoraEndPoint : BaseAsyncEndpoint.WithRequest<ConstructoraDto>
        .WithResponse<Response<ConstructoraDto>>
    {
        private readonly IConstructoraBussines _constructora;

        public AgregrarConstructoraEndPoint(IConstructoraBussines convocatoriaBussigneLogic)
        {
            _constructora = convocatoriaBussigneLogic;
        }
        [HttpPost("api/Constructora/AgregarConstructora")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Agregar una constructora",
            Description = "Agregar una constructora",
            OperationId = "Constructora.AgregarConstructora",
            Tags = new[] { "ConstructoraEndPoint" })]
        public async override Task<ActionResult<Response<ConstructoraDto>>> HandleAsync(ConstructoraDto convocatoria, CancellationToken cancellationToken = default)
        {
            return await _constructora.AgregaConstructora(convocatoria);
        }
    }
}
