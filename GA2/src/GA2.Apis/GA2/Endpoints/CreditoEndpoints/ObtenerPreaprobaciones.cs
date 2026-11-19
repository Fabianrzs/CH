using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.EmbargosEndPoints
{
    /// <summary>
    /// Endpoint para Obtener Flujo Tipo Credito Por Id
    /// </summary>
    /// <author>Cristian Gonzalez Parra</author>
    /// <date>11/05/2021</date>
    [Authorize]
    public class ObtenerPreaprobaciones : BaseAsyncEndpoint.WithRequest<RequestPreaprobacionesDto>.WithResponse<Response<IEnumerable<PreaprobacionesDto>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor del Endpoint Obtener preaprobaciones filtros Credito
        /// </summary>
        /// <author>Nelson Redondo Barros</author>
        /// <date>24/11/2022</date>

        public ObtenerPreaprobaciones(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/ObtenerPreaprobaciones")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<PreaprobacionesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
         Summary = "Historial de Preaprobaciones",
         Description = "Historial de Preaprobaciones",
         OperationId = "CreditoEndpoint.ObtenerPreaprobaciones",
         Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<PreaprobacionesDto>>>> HandleAsync(RequestPreaprobacionesDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerPreaprobaciones(request);
        }
    }
}
