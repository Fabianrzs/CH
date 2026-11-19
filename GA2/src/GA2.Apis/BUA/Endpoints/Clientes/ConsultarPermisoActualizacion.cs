using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.BUA;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    /// <summary>
    /// Endpoint que consulta permiso para actualizar los datos del usuario
    /// <author>Michel Parra</author>
    /// </summary>
    [Authorize]
    public class ConsultarPermisoActualizacion : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<ConsultarPermisoActualizacionDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public ConsultarPermisoActualizacion(IClientesBusinessLogic clientsBusinessLogic)
        {
            _clientsBusinessLogic = clientsBusinessLogic;
        }
        /// <summary>
        /// Update client info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        //[ValidateAntiForgeryToken]
        [HttpGet("api/ConsultarPermisoActualizacion")]
        [ProducesResponseType(typeof(Response<ConsultarPermisoActualizacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Consulta permiso para actualizar los datos de un cliente",
           Description = "Consulta permiso para actualizar",
           OperationId = "client.ConsultarPermisoActualizacion",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<ConsultarPermisoActualizacionDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ConsultarPermisoActualizacion(request);
        }
    }
}
