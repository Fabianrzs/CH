using System;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{

    /// <summary>
    /// Endpoint para Anular una solicitud
    /// </summary>
    /// <author>David Pineda</author>
    /// <date>23/09/2022</date>
    [Authorize]
    public class AnularSolicitudTarea :
         BaseAsyncEndpoint.WithRequest<RechazarSolicitudTareaDto>
        .WithResponse<Response<bool>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Anular solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>David Pineda</author>
        /// <date>23/09/2022</date>
        public AnularSolicitudTarea(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Punto de entrada
        /// </summary>
        /// <param name="rechazarSolicitudTareaDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/AnularSolicitudTarea")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Anular Solicitud",
           Description = "Anular Solicitud de una tarea",
           OperationId = "Solicitud.AnularSolicitudTarea",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(RechazarSolicitudTareaDto rechazarSolicitudTareaDto, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.AnularSolicitudTarea(rechazarSolicitudTareaDto);
        }

    }
}

