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
    public class ConvocatoriaSinCupoEndPoint : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<IEnumerable<PostulanteConvocatoriaDto>>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public ConvocatoriaSinCupoEndPoint(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }
        [HttpGet("api/Convocatoria/ConvocatoriaSinCupo")]
        [ProducesResponseType(typeof(Response<IEnumerable<PostulanteConvocatoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos los postulante sin cupo de las convocatoria",
            Description = "Obtiene todos los postulante de las  convocatoria ",
            OperationId = "convocatoria.ConvocatoriaSinCupo",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<PostulanteConvocatoriaDto>>>> HandleAsync(int idConvocatoria, CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.GetConvocatoriaSinCupo(idConvocatoria);
        }
    }
}
