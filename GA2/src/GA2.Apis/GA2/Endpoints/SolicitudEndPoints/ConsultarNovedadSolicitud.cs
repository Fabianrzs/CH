using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint para consultar novedades en seguridad documental.
    /// </summary>
    public class ConsultarNovedadSolicitud : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<List<ConsultarNovedad>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Contructor para consultar novedades en seguridad documental.
        /// </summary>
        public ConsultarNovedadSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Contructor para consultar novedades en seguridad documental.
        /// </summary>
        [HttpGet("api/solicitud/ConsultarNovedadSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<List<ConsultarNovedad>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar novedades solicitud",
            Description = "Consultas las novedades de las tareas de seguridad documental",
            OperationId = "Solicitud.ConsultarNovedadSolicitud",
            Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<List<ConsultarNovedad>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarNovedadSolicitud();

        }
    }
}
