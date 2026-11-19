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
    public class ConsultarConstructoraById : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<ConstructoraDto>>
    {
        private readonly IConstructoraBussines _constructora;

        public ConsultarConstructoraById(IConstructoraBussines convocatoriaBussigneLogic)
        {
            _constructora = convocatoriaBussigneLogic;
        }
        [HttpPost("api/Constructora/ConsultarConstructoraById")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar Constructora by Id",
            Description = "Consultar Constructora by Id",
            OperationId = "Constructora.ConsultarConstructoraById",
            Tags = new[] { "ConstructoraEndPoint" })]
        public async override Task<ActionResult<Response<ConstructoraDto>>> HandleAsync(Guid IdConstructora, CancellationToken cancellationToken = default)
        {
            return await _constructora.ConsultarConstructoraById(IdConstructora);
        }
    }
}
