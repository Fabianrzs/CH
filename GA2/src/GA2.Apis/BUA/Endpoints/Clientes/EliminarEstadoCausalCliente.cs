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
    /// Endpoint que elimina estado y causal del cliente
    /// <author>Michel Parra</author>
    /// </summary>
    [Authorize]
    public class EliminarEstadoCausalCliente : BaseAsyncEndpoint.WithRequest<CausalEstadoClienteDto>.WithResponse<Response<CausalEstadoClienteDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public EliminarEstadoCausalCliente(IClientesBusinessLogic clientsBusinessLogic)
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
        [HttpPost("api/Clientes/EliminarEstadoCausalCliente")]
        [ProducesResponseType(typeof(Response<CausalEstadoClienteDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Elimina estado y causal del cliente",
           Description = "Elimina estado y causal del cliente",
           OperationId = "client.EliminarEstadoCausalCliente",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<CausalEstadoClienteDto>>> HandleAsync(CausalEstadoClienteDto request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.EliminarEstadoCausalCliente(request);
        }
    }
}
