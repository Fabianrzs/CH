using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Constructora;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Constuctora
{
    public class EliminarConstructora : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<string>>
    {
        private readonly IConstructoraBussines _constructora;

        public EliminarConstructora(IConstructoraBussines convocatoriaBussigneLogic)
        {
            _constructora = convocatoriaBussigneLogic;
        }
        [HttpPost("api/Constructora/EliminarConstructoraById")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "eliminar Constructora by Id",
            Description = "eliminar Constructora by Id",
            OperationId = "Constructora.EliminarConstructoraById",
            Tags = new[] { "ConstructoraEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(Guid IdConstructora, CancellationToken cancellationToken = default)
        {
            return await _constructora.EliminarConstructoraById(IdConstructora);
        }
    }
}
