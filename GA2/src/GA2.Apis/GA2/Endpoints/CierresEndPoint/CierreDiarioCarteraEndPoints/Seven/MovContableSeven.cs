using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Mov Contable Seven del CierreDiarioCartera
    /// </summary>
    [Authorize]
    public class MovContableSeven : BaseAsyncEndpoint
        .WithRequest<CierreDiarioDto>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public MovContableSeven(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/MovContableSevenCierreDiario")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<InsertarMovContableResponse>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Ejecutar Mov. Contable en Seven del Cierre Diario",
           Description = "Ejecutar Mov. Contable en Seven Cierre Diario",
           OperationId = "Credito.MovContableSevenCierreDiarioCartera",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(CierreDiarioDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CrearMovContableCierreDiarioSeven(request, cancellationToken);
        }
    }
}
