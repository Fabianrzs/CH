using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class AsignacionDeTareas :
         BaseAsyncEndpoint.WithRequest<TareasSolicitudDto>
        .WithResponse<Response<TareasSolicitudDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>02/06/2022</date>
        public AsignacionDeTareas(ISolicitudBusinessLogic solicitudBusinessLogic)
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
        [HttpPost("api/AsignacionDeTareas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<TrazabilidadSolicitudDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "AsignacionDeTareas",
           Description = "AsignacionDeTareas",
           OperationId = "Solicitud.AsignacionDeTareas",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<TareasSolicitudDto>>> HandleAsync(TareasSolicitudDto idUsuario, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.AsignacionDeTareas(idUsuario);
        }
    }
}
