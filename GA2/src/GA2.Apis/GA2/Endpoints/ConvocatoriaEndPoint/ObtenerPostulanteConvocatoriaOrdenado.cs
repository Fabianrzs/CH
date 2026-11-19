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
    public class ObtenerPostulanteConvocatoriaOrdenado : BaseAsyncEndpoint.WithRequest<int>
        .WithResponse<Response<IEnumerable<PostulanteConvocatoriaDto>>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public ObtenerPostulanteConvocatoriaOrdenado(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }
        [HttpGet("api/Convocatoria/ConsultarPostulanteConvocatoriaOrdenByPuntaje")]
        [ProducesResponseType(typeof(Response<IEnumerable<PostulanteConvocatoriaDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos los postulante de las convocatoria",
            Description = "Obtiene todos los postulante de las  convocatoria ",
            OperationId = "convocatoria.ConsultarPostulanteConvocatoriaOrdenByPuntaje",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<PostulanteConvocatoriaDto>>>> HandleAsync(int idConvocatoria, CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.ConsultarPostulanteByConvocatoriaOrden(idConvocatoria);
        }
    }
}
