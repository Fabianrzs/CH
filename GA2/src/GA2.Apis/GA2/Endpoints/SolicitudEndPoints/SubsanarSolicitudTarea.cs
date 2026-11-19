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
    /// Endpoint para subsanar una actividad
    /// </summary>
    /// <author>David Pineda</author>
    /// <date>29/10/2022</date>
    [Authorize]
    public class SubsanarSolicitudTarea :
         BaseAsyncEndpoint.WithRequest<ActualizarSolicitudTareaDto>
        .WithResponse<Response<bool>>
    {
        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Subsanar solicitud
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>David Pineda</author>
        /// <date>20/10/2021</date>
        public SubsanarSolicitudTarea(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SubsanarSolicitudTarea"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("api/solicitud/SubsanarSolicitudTarea")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Subsanar Solicitud de una tarea",
           Description = "Subsanar Solicitud de de una tarea nueva",
           OperationId = "Solicitud.SubsanarSolicitudTarea",
           Tags = new[] { "SolicitudEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(ActualizarSolicitudTareaDto actualizarSolicitudTarea, CancellationToken cancellationToken = default)
        {
            return await this._solicitudBusinessLogic.SubsanarSolicitudTarea(actualizarSolicitudTarea);
        }
    }
}

