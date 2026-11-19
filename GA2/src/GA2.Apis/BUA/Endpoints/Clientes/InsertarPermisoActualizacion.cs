using Ardalis.ApiEndpoints;
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
    /// Endpoint que inserta permiso para actualizar los datos del usuario
    /// <author>Michel Parra</author>
    /// </summary>
    [Authorize]
    public class InsertarPermisoActualizacion : BaseAsyncEndpoint.WithRequest<InsertarPermisoActualizacionDto>.WithResponse<Response<ResponseInsertarPermisoActualizacionDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public InsertarPermisoActualizacion(IClientesBusinessLogic clientsBusinessLogic)
        {
            _clientsBusinessLogic = clientsBusinessLogic;
        }
        /// <summary>
        /// Update client info
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/InsertarPermisoActualizacion")]
        [ProducesResponseType(typeof(Response<ResponseInsertarPermisoActualizacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta permiso para actualizar los datos de un cliente",
           Description = "Inserta permiso para actualizar",
           OperationId = "client.InsertarPermisoActualizacion",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<ResponseInsertarPermisoActualizacionDto>>> HandleAsync(InsertarPermisoActualizacionDto request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.InsertarPermisoActualizacion(request);
        }
    }
}
