using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class ConsultarAsignacionTareas :
         BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<TareasSolicitudDto>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>02/06/2022</date>
        public ConsultarAsignacionTareas(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/ConsultarAsignacionTareas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<TrazabilidadSolicitudDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "ConsultarAsignacionTareas",
           Description = "ConsultarAsignacionTareas",
           OperationId = "Solicitud.ConsultarAsignacionTareas",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TareasSolicitudDto>>>> HandleAsync(Guid idUsuario, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ConsultarAsignacionTareas(idUsuario);
        }
    }
}
