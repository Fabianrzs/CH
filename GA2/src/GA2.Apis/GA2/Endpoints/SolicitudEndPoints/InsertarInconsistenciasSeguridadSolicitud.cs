using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SolicitudEndPoints
{

    /// <summary>
    /// Endpoint para Crear una inconsistencia de solicitud
    /// </summary>
    public class InsertarInconsistenciasSeguridadSolicitud : BaseAsyncEndpoint.WithRequest<InconsistenciaSolicitudDto>
        .WithResponse<Response<InconsistenciaSolicitudDto>>
    {

        ISolicitudBusinessLogic _solicitudBusinessLogic; 

        /// <summary>
        /// Constructor para Crear una inconsistencia de solicitud
        /// </summary>
        public InsertarInconsistenciasSeguridadSolicitud(ISolicitudBusinessLogic solicitudBusinessLogic)
        {
            _solicitudBusinessLogic = solicitudBusinessLogic; 
        }

        /// <summary>
        /// EndPoint para crear una inconsistencia de solicitud
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/solicitud/InsertarInconsistenciasSeguridadSolicitud")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InconsistenciaSolicitudDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Insertar Inconsistencia seguridad documental",
           Description = "Insertar Inconsistencia seguridad documental",
           OperationId = "Solicitud.InsertarInconsistenciasSeguridadSolicitud",
           Tags = new[] { "SolicitudEndpoint" })]
        public override async Task<ActionResult<Response<InconsistenciaSolicitudDto>>> HandleAsync(InconsistenciaSolicitudDto request, CancellationToken cancellationToken = default)
        {
            return await _solicitudBusinessLogic.InsertarInconsistenciaSeguridadSolicitud(request); 
        }
    }
}
