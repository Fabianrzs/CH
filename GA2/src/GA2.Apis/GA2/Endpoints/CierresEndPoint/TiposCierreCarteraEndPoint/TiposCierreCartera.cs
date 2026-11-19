using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Cartera.Cierres;

namespace GA2.Apis.GA2.Endpoints.CierresEndPoint.TiposCierreCarteraEndPoint
{
    /// <summary>
    /// TiposCierreCartera
    /// </summary>
    [Authorize]
    public class TiposCierreCartera : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<TipoCierreCarteraDto>>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public TiposCierreCartera(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerTiposDeCierreCartera")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<TipoCierreCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene los tipos de cierre de cartera",
           Description = "Obtiene los tipos de cierre de cartera",
           OperationId = "Credito.ObtenerTiposDeCierreCartera",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TipoCierreCarteraDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerTiposDeCierreCartera();
        }
    }
}
