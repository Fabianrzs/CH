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

    /// <summary>
    /// ObtenerValorDesembolsoCredito
    /// </summary>
    [Authorize]
    public class ObtenerValorDesembolsoCredito : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<Response<ValorDesembolsoCreditoDto>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Obtiene valor del desembolso del credito
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        public ObtenerValorDesembolsoCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }


        /// <summary>
        /// Obtiene valor del desembolso del credito
        /// </summary>
        /// <param name="numeroDocumento"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerValorDesembolsoCredito")]
        [ProducesResponseType(typeof(Response<ValorDesembolsoCreditoDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene valor del desembolso del credito",
            Description = "Obtiene valor del desembolso del credito",
            OperationId = "credito.ObtenerValorDesembolsoCredito",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<ValorDesembolsoCreditoDto>>> HandleAsync(string numeroDocumento, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerValorDesembolsoCredito(numeroDocumento);
        }

    }
}
