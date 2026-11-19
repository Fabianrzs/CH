using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Clase del endpoint para consultar el radicado de la solicitud 
    /// </summary>
    [Authorize]
    public class ConsultarRadicadoSolicitud :
         BaseAsyncEndpoint.WithRequest<ConsultarRadicadoSolicitudDto>
        .WithResponse<Response<RespuestaConsultarRadicadoSolicitud>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar el radicado de la solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ConsultarRadicadoSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }


        /// <summary>
        /// EndPoint para consultar el radicado de la solicitud 
        /// </summary>
        /// <param name="solicitudDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/ConsultarRadicadoSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<RespuestaConsultarRadicadoSolicitud>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar radicado solicitud",
           Description = "Consultar radicado solicitud",
           OperationId = "Solicitud.ConsultarRadicadoSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<RespuestaConsultarRadicadoSolicitud>>> HandleAsync(ConsultarRadicadoSolicitudDto solicitudDto, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarRadicadoSolicitud(solicitudDto); 
        }
    }
}
