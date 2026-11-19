using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class ObtenerListConvocatoria : BaseAsyncEndpoint.WithoutRequest
        .WithResponse<Response<IEnumerable<int>>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public ObtenerListConvocatoria(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }
        [HttpGet("api/Convocatoria/ConsultarListConvocatoriasId")]
        [ProducesResponseType(typeof(Response<ConvocatoriaDTO>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene todos los id de las convocatoria",
            Description = "Obtiene todos los id de las  convocatoria ",
            OperationId = "convocatoria.ConsultarListConvocatoriasId",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<int>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.Obtener_List_Convocatoria();
        }

    }
}
