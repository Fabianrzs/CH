using System.Linq;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using GA2.Domain.Entities.Solicitud;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{
    /// <summary>
    /// Endpoint carga documento solicitud de cesantías
    /// </summary>
    [Authorize]
    public class ObtenerDocumentosChecks : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<CheckArchivo>>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Cargar documento
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Erwin Pantoja</author>
        /// <date>20/10/2021</date>
        public ObtenerDocumentosChecks(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Carga Archivo al blob storage para solicitud de cesantías
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpGet("api/solicitud/obtenerDocumentosChecks")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Documentos Checks",
           Description = "Obtener Documentos Checks",
           OperationId = "Solicitud.obtenerDocumentosChecks",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CheckArchivo>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ObtenerArchivosCheck(request);
        }
    }
}
