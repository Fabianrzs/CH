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
    /// <summary>
    /// Endpoint para consultar las inconsistencias de una solicitud
    /// </summary>
    public class ConsultarInconsistenciasSeguridadSolicitud : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<InconsistenciaSolicitudDto>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;
        /// <summary>
        /// Endpoint para consultar las inconsistencias de una solicitud
        /// </summary>
        public ConsultarInconsistenciasSeguridadSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic; 
        }


        /// <summary>
        /// Consulta las inconsistencias de una solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/solicitud/ConsultarInconsistenciaSeguridadSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar Inconsistencia por solicitud",
           Description = "Consultar Inconsistencia por solicitud",
           OperationId = "Solicitud.ConsultarInconsistenciaSeguridadSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<InconsistenciaSolicitudDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarInconsistenciaSeguridadSolicitud(request); 
        }
    }
}
