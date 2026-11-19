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
    /// Endpoint para aprobar solicitud de cargue de documentos.
    /// </summary>
    /// <author>Anderson Páez</author>
    /// <date>7/09/2022</date>
    [Authorize]
    public class AprobarSolicitudTareaDocumento :
         BaseAsyncEndpoint.WithRequest<AprobarSolicitudTareaDto>
        .WithResponse<Response<ObtenerTramiteSolicitudDto>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Aprobar solicitud tarea de cargue de documentos
        /// </summary>
        /// <author>Anderson Páez</author>
        /// <param name="solicitudBusinessLogic"></param>
        /// <date>7/09/2022</date>
        public AprobarSolicitudTareaDocumento(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// Metodo Pots para aprobar solicitud de cargue de documentos.
        /// </summary>
        /// <author>Anderson Páez</author>
        /// <param name="aprobarSolicitudTareaDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/AprobarSolicitudTareaDocumento")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ObtenerTramiteSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Aprobar Solicitud de una tarea de cargue de documentos",
           Description = "Aprobar Solicitud de una tarea de cargue de documentos",
           OperationId = "Solicitud.AprobarSolicitudTareaDocumento",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<ObtenerTramiteSolicitudDto>>> HandleAsync(AprobarSolicitudTareaDto aprobarSolicitudTareaDto, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.AprobarSolicitudTareaDocumento(aprobarSolicitudTareaDto);
        }

    }

}
