using Ardalis.ApiEndpoints;
using GA2.Domain.Entities.Solicitud;
using System.Collections.Generic;
using System;
using GA2.Transversals.Commons;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para obtener informes 114 - incosistencias de seguridad documental grupo 1 y grupo 2
    /// </summary>
    public class ConsultarInformeInconsistenciasSolicitud : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<ConsultarInformeInconsistenciaSolicitud>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsultarInformeInconsistenciasSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Obtener los tipos de informes 114
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/ConsultarInformeInconsistenciasSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ConsultarInformeInconsistenciaSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "informes 114 inconsistencia",
           Description = "Obtener los informes 114 inconsistencia",
           OperationId = "Solicitud.ConsultarInformeInconsistenciasSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ConsultarInformeInconsistenciaSolicitud>>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarInformeInconsistenciaSolicitud(request); 
        }
    }
}
