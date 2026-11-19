using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class AnularConvocatoriaEndPoint : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<string>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public AnularConvocatoriaEndPoint(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }
        [HttpGet("api/Convocatoria/AnularConvocatoria")]
        [ProducesResponseType(typeof(Response<string>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Anular las convocatoria by id",
            Description = "Anular las convocatoria by id",
            OperationId = "convocatoria.ConsultarPostulanteConvocatoria",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<string>>> HandleAsync(int idConvocatoria, CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.AnularConvocatoria(idConvocatoria);
        }
    }
}
