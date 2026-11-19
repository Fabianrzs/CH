using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.SolicitudConvocatoria;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Application.Dto.Postulacion;
using GA2.Transversals.Commons;
using GA2.Domain.Core.Postulacion;
using System.Linq;
using System;

namespace GA2.Apis.GA2.Endpoints.Postulacion
{
    public class ConsultarSolicitudPostulacionEndPoint : BaseAsyncEndpoint.WithRequest<SolicitudPostulacionRequestDto>
        .WithResponse<Response<IEnumerable<SolicituPostulancionResponseDto>>>
    {
        private readonly IPostulacionBussineLogic _postulacionBussineLogic;
        //private const string USR_ID = "USR_ID";

        public ConsultarSolicitudPostulacionEndPoint(IPostulacionBussineLogic postulacionBussineLogic)
        {
            _postulacionBussineLogic = postulacionBussineLogic;
        }


        [HttpGet("api/postulante/consultarSolicutdPostulacion")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConvocatoriaResponseDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene las solcitud de postulacion consultada",
            Description = "Obtiene las solcitud de postulacion consultadas",
            OperationId = "postulante.consultarSolicutdPostulacion",
            Tags = new[] { "PostulanteEndPoint" })]
        public async override Task<ActionResult<Response<IEnumerable<SolicituPostulancionResponseDto>>>> HandleAsync([FromQuery] SolicitudPostulacionRequestDto request, CancellationToken cancellationToken = default)
        {
            return await _postulacionBussineLogic.ConsultarSolicitudPostultacion(request);
        }
    }
}
