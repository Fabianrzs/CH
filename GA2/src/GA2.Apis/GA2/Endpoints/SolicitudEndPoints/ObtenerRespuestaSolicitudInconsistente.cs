using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    /// <summary>
    /// Obtener Respuesta Embargo
    /// </summary>
    ///     
    [Authorize]

    public class ObtenerRespuestaSolicitudInconsistente : BaseAsyncEndpoint
        .WithRequest<ConsultarSolicitudDto>
        .WithResponse<Response<FileResult>>
    {

        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        public ObtenerRespuestaSolicitudInconsistente(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ObtenerRespuestaEmbargo
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ObtenerRespuestaSolicitudInconsistente")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Reporte Obtener Respuesta Solicitud Inconsistente",
           Description = "Reporte Obtener Respuesta Solicitud Inconsistente",
           OperationId = "Embargos.ObtenerRespuestaSolicitudInconsistente",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(ConsultarSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await this._reportesBusinessLogic.ObtenerRespuestaSolicitudInconsistente(request);
        }
    }
}
