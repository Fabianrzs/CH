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

namespace GA2.Apis.GA2.Endpoints
{

    public class ObtenerEstadoSolicCredito : BaseAsyncEndpoint.WithRequest<string>.WithResponse<Response<EstadoSolicCreditoDto>>
    {

        private readonly ICreditoBusinessLogic _ICreditoBusinessLogic;

        public ObtenerEstadoSolicCredito(ICreditoBusinessLogic iCreditoBusinessLogic)
        {
            _ICreditoBusinessLogic = iCreditoBusinessLogic;
        }



        /// <summary>
        /// Obtiene all ciudades
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerEstadoSolicCredito")]
        [ProducesResponseType(typeof(Response<IEnumerable<EstadoSolicCreditoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene Estado Solicitud de credito",
            Description = "Obtiene Estado Solicitud de credito",
            OperationId = "credito.ObtenerEstadoSolicCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<EstadoSolicCreditoDto>>> HandleAsync(string request, CancellationToken cancellationToken = default)
        {
            return await this._ICreditoBusinessLogic.ObtenerEstadoSolicCredito(request);
  
        }
    }
}
