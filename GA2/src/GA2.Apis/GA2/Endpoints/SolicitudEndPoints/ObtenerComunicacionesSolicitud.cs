using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    [Authorize]

    public class ObtenerComunicacionesSolicitud :
         BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<ComunicacionSolicitudDto>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>02/06/2022</date>
        public ObtenerComunicacionesSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="idSolicitud"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/solicitud/ObtenerComunicacionesSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ComunicacionSolicitudDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener comunicaciones solicitud",
           Description = "Obtener comunicaciones solicitud",
           OperationId = "Solicitud.ObtenerComunicacionesSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ComunicacionSolicitudDto>>>> HandleAsync(Guid idSolicitud, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ObtenerComunicacionesSolicitud(idSolicitud);
        }
    }
}
