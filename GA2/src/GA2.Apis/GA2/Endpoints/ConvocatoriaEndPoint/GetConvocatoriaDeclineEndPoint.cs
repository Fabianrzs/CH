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
    public class GetConvocatoriaDeclineEndPoint : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<IEnumerable<ConvocatoriaDeclineDto>>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public GetConvocatoriaDeclineEndPoint(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }
        [HttpGet("api/Convocatoria/GetConvocatoriaRechazada")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConvocatoriaDeclineDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos los postulante rechazada de las convocatoria",
            Description = "Obtiene todos los postulante de las  convocatoria ",
            OperationId = "convocatoria.GetConvocatoriaRechazada",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ConvocatoriaDeclineDto>>>> HandleAsync(int idConvocatoria, CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.GetConvocatoriaDeline(idConvocatoria);
        }
    }
}
