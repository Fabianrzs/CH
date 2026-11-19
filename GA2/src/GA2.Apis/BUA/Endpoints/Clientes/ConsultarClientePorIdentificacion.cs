using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    [Authorize]
    public class ConsultarClientePorIdentificacion : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<IEnumerable<ClienteIdentificacionDto>>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        public ConsultarClientePorIdentificacion(IClientesBusinessLogic clientsBusinessLogic)
        {
            _clientsBusinessLogic = clientsBusinessLogic;
        }


        [HttpGet("api/clientes/ConsultarClientePorIdentificacion")]
        [ProducesResponseType(typeof(Response<ClienteIdentificacionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
          Summary = "Obtiene todos los cliente por identificacion",
          Description = "Obtiene todos los cliente por identificacion",
          OperationId = "client.ConsultarClientePorIdentificacion",
          Tags = new[] { "ClientEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ClienteIdentificacionDto>>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _clientsBusinessLogic.ConsultarClientePorIdentificacion(request);
        }
    }
}
