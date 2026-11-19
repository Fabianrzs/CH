using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    public class ObtenetClientesPorUnidadEjecutora : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<IEnumerable<ClienteDto>>>
    {
        IClientesBusinessLogic _clientesBusinessLogic;

        public ObtenetClientesPorUnidadEjecutora(IClientesBusinessLogic clientesBusinessLogic)
        {
            _clientesBusinessLogic = clientesBusinessLogic;
        }



        [HttpPost("api/ObtenetClientesPorUnidadEjecutora")]
        [ProducesResponseType(typeof(Response<IEnumerable<ClienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta un embargo",
           Description = "Inserta un embargo",
           OperationId = "Embargos.InsertarEmbargo",
           Tags = new[] { "EmbargosEndPoints" })]
        public override async Task<ActionResult<Response<IEnumerable<ClienteDto>>>> HandleAsync(int idUnidadEjecutora, CancellationToken cancellationToken = default)
        {
            return await this._clientesBusinessLogic.ObtenetClientesPorUnidadEjecutora(idUnidadEjecutora);
        }
    }
}
