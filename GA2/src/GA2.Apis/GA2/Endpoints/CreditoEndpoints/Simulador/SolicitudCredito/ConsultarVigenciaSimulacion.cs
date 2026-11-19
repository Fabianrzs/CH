using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito.Simulador.SolicitudCredito;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints.Simulador.SolicitudCredito
{
    public class ConsultarVigenciaSimulacion : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<bool>>
    {
        private readonly IProspeccionBusinessLogic _prospeccionBusinessLogic;
        public ConsultarVigenciaSimulacion(IProspeccionBusinessLogic prospeccionBusinessLogic)
        {
            _prospeccionBusinessLogic = prospeccionBusinessLogic;
        }

        /// <summary>
        /// crea simulacion del cliente
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("/api/prospeccion/ConsultarVigenciaSimulacion")]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        [SwaggerOperation("Consultar Vigencia Simulacion Cliente",
            Description = "Consultar Vigencia Simulacion Clienteo",
            OperationId = "prospeccion.ConsultarVigenciaSimulacion",
            Tags = new[] { "ProspeccionEndpoint" })]
        public async override Task<ActionResult<Response<bool>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await _prospeccionBusinessLogic.ConsultarVigenciaSimulacion(request);
        }
    }
}
