using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.BUA.Endpoints
{
    [Authorize]

    public class ObtenerCuotasNumeroIdentificacion : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<ResponseDatosSimulacionParaEstudioDto>>
    {
        private readonly IClientesBusinessLogic _clientsBusinessLogic;

        public ObtenerCuotasNumeroIdentificacion(IClientesBusinessLogic clientsBusinessLogic)
        {
            _clientsBusinessLogic = clientsBusinessLogic;
        }


        [HttpGet("api/clientes/ObtenerCuotasNumeroIdentificacion")]
        [ProducesResponseType(typeof(Response<ResponseDatosSimulacionParaEstudioDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene la cumas de cuotas",
           Description = "Obtiene la cumas de cuotas",
           OperationId = "client.ObtenerCuotasNumeroIdentificacion",
           Tags = new[] { "ClientEndpoint" })]
        public override async Task<ActionResult<Response<ResponseDatosSimulacionParaEstudioDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await this._clientsBusinessLogic.ObtenerCuotasNumeroIdentificacion(request);
          
        }
    }
}
