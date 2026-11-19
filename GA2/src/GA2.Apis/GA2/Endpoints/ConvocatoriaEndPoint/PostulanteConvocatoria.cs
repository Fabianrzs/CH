using Ardalis.ApiEndpoints;

using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto;
using GA2.Transversals.Commons;
using System;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class PostulanteConvocatoria : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<IEnumerable<PostulanteConvocatoriaDto>>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public PostulanteConvocatoria(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }
        [HttpGet("api/Convocatoria/ConsultarPostulanteConvocatoria")]
        [ProducesResponseType(typeof(Response<IEnumerable<PostulanteConvocatoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos los postulante de las convocatoria",
            Description = "Obtiene todos los postulante de las  convocatoria ",
            OperationId = "convocatoria.ConsultarPostulanteConvocatoria",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<PostulanteConvocatoriaDto>>>> HandleAsync(int idConvocatoria,CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.ConsultarPostulanteByConvocatoria(idConvocatoria);
        }
    }
}
