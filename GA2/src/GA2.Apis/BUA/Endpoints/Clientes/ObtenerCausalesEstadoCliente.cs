using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Domain.Core;
using GA2.Application.Dto.BUA;
using GA2.Transversals.Commons;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    /// <summary>
    /// Endpoint que obtiene causales estado del cliente
    /// <author>Michel Parra</author>
    /// </summary>
    [Authorize]
    public class ObtenerCausalesEstadoCliente : BaseAsyncEndpoint.WithRequest<CausalEstadoClienteDto>.WithResponse<Response<IEnumerable<CausalEstadoClienteDto>>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="clientsBusinessLogic"></param>
        public ObtenerCausalesEstadoCliente(IClientesBusinessLogic clientsBusinessLogic)
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
        [HttpPost("api/Clientes/ObtenerCausalesEstadoCliente")]
        [ProducesResponseType(typeof(Response<IEnumerable<CausalEstadoClienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene causales estado del cliente",
           Description = "Obtiene causales estado del cliente",
           OperationId = "client.ObtenerCausalesEstadoCliente",
           Tags = new[] { "ClientEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CausalEstadoClienteDto>>>> HandleAsync(CausalEstadoClienteDto request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ObtenerCausalesEstadoCliente(request); 
        }
    }
}
