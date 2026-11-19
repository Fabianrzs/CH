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
    /// Endpoint que elimina anotaciones del cliente
    /// <author>Michel Parra</author>
    /// </summary>
    [Authorize]
    public class EliminarAnotacionCliente : BaseAsyncEndpoint.WithRequest<AnotacionClienteDto>.WithResponse<Response<AnotacionClienteDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public EliminarAnotacionCliente(IClientesBusinessLogic clientsBusinessLogic)
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
        [HttpPost("api/Clientes/EliminarAnotacionCliente")]
        [ProducesResponseType(typeof(Response<AnotacionClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Elimina anotaciones del cliente",
           Description = "Elimina anotaciones del cliente",
           OperationId = "client.EliminarAnotacionCliente",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<AnotacionClienteDto>>> HandleAsync(AnotacionClienteDto request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.EliminarAnotacionCliente(request);
        }
    }
}
