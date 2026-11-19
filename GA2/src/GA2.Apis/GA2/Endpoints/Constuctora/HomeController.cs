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
    public class HomeController : BaseAsyncEndpoint.WithRequest<ConstructoraDto>
        .WithResponse<Response<string>>
    {
        private readonly IConstructoraBussines _constructora;

        public HomeController(IConstructoraBussines convocatoriaBussigneLogic)
        {
            _constructora = convocatoriaBussigneLogic;
        }
        [HttpPost("api/Constructora/ActualizarConstructora")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualizar una constructora",
            Description = "Actualizar una constructora",
            OperationId = "Constructora.ActualizarConstructora",
            Tags = new[] { "ConstructoraEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(ConstructoraDto convocatoria, CancellationToken cancellationToken = default)
        {
            return await _constructora.ActualizarConstructora(convocatoria);
        }
    }
}
