using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Solicitud;
using GA2.Domain.Core;
using GA2.Domain.Entities.Solicitud;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Clase del endpoint para consultar por nombres y apellidos los asesores
    /// </summary>
    [Authorize]
    public class ConsultarUsuarioIngresoSolicitud :
         BaseAsyncEndpoint.WithRequest<RespuestaConsultarUsuarioIngresoSolicitud>
        .WithResponse<Response<IEnumerable<UsuarioIngresoSolicitud>>>
    {

        private readonly ISolicitudBusinessLogic _solicitudBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint para consultar por nombres y apellidos los asesores
        /// </summary>
        /// <param name="solicitudBusinessLogic"></param>
        /// <author>Alejandro Lara</author>        
        public ConsultarUsuarioIngresoSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic;
        }



        /// <summary>
        /// EndPoint para consultar por nombres y apellidos los asesores
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/solicitud/ConsultarUsuarioIngresoSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<UsuarioIngresoSolicitud>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consultar asesor por nombre o apellido",
           Description = "Consultar asesores por nombre o apellido",
           OperationId = "Solicitud.ConsultarUsuarioIngresoSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<UsuarioIngresoSolicitud>>>> HandleAsync(RespuestaConsultarUsuarioIngresoSolicitud request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.ConsultarUsuarioSolicitud(request); 
        }
    }
}
