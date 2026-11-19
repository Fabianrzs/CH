using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    public class ObtenerTrazabilidadSolicitud :
         BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<TrazabilidadSolicitudDto>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para Actualizar una inconsistencia de solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Hanson Restrepo</author>
        /// <date>02/06/2022</date>
        public ObtenerTrazabilidadSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
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
        [HttpGet("api/solicitud/ObtenerTrazabilidadSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<TrazabilidadSolicitudDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Trazabilidad Solicitud",
           Description = "Obtener Trazabilidad Solicitud",
           OperationId = "Solicitud.ObtenerTrazabilidadSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TrazabilidadSolicitudDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.ObtenerTrazabilidadSolicitud(request);
        }
    }
}
