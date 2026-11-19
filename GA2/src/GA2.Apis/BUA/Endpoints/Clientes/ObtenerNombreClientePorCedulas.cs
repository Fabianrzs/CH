using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;

namespace GA2.Apis.BUA.Endpoints.Clientes
{
    public class ObtenerNombreClientePorCedulas : BaseAsyncEndpoint.WithRequest<List<string>>.WithResponse<Response<IEnumerable<ClienteDto>>>
    {
        private readonly IClientesBusinessLogic _clientesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerNombreClientePorCedulas(IClientesBusinessLogic clientesBusinessLogic)
        {
            _clientesBusinessLogic = clientesBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Clientes/ObtenerNombreClientePorCedulas")]
        [ProducesResponseType(typeof(Response<IEnumerable<ClienteDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar tipo actor by id",
            Description = "Consulta la excepcion segun el id",
            OperationId = "credito.ConsultarActor",
            Tags = new[] { "CarteraEndpoint" })]

        public async override Task<ActionResult<Response<IEnumerable<ClienteDto>>>> HandleAsync(List<string> request, CancellationToken cancellationToken = default)
        {
            return await this._clientesBusinessLogic.ObtenerNombreClientePorCedulas(request);
        }
    }
}
