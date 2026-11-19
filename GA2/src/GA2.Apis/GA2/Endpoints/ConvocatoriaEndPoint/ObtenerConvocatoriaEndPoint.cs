using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.SolicitudConvocatoria;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace GA2.Apis.GA2.Endpoints.ConvocatoriaEndPoint
{
    public class ObtenerConvocatoriaEndPoint : BaseAsyncEndpoint.WithRequest<ConsultarConvocatoriaRequest>
        .WithResponse<Response<IEnumerable<ConvocatoriaResponseDto>>>
    {
        private readonly IConvocatoriaBussigneLogic _convocatoriaBussigneLogic;

        public ObtenerConvocatoriaEndPoint(IConvocatoriaBussigneLogic convocatoriaBussigneLogic)
        {
            _convocatoriaBussigneLogic = convocatoriaBussigneLogic;
        }


        [HttpGet("api/convocatoria/obtenerConvocatoria")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConvocatoriaResponseDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene las convocatoria consultadas",
            Description = "Obtiene las convocatoria consultadas",
            OperationId = "convocatoria.obtenerConvocatoria",
            Tags = new[] { "ConvocatoriaEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ConvocatoriaResponseDto>>>> HandleAsync([FromQuery] ConsultarConvocatoriaRequest request, CancellationToken cancellationToken = default)
        {
            return await _convocatoriaBussigneLogic.ConsultarConvocatoria(request);
        }
    }
}
