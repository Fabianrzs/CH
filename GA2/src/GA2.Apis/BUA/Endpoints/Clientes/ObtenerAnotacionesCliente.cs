using Ardalis.ApiEndpoints;
using GA2.Application.Dto.BUA;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    /// <summary>
    /// Endpoint que obtiene anotaciones del cliente
    /// <author>Michel Parra</author>
    /// </summary>
    [Authorize]
    public class ObtenerAnotacionesCliente : BaseAsyncEndpoint.WithRequest<AnotacionClienteDto>.WithResponse<Response<IEnumerable<AnotacionClienteDto>>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public ObtenerAnotacionesCliente(IClientesBusinessLogic clientsBusinessLogic)
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
        [HttpPost("api/Clientes/ObtenerAnotacionesCliente")]
        [ProducesResponseType(typeof(Response<IEnumerable<AnotacionClienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene anotaciones del cliente",
           Description = "Obtiene anotaciones del cliente",
           OperationId = "client.ObtenerAnotacionesCliente",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<AnotacionClienteDto>>>> HandleAsync(AnotacionClienteDto request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ObtenerAnotacionesCliente(request);
        }
    }
}
