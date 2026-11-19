using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Cierres.RequestCierreDiarioCartera;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.CierresEndPoint.CierreDiarioCarteraEndPoints.Seven
{
    /// <summary>
    /// CierreDiarioCartera
    /// </summary>
    [Authorize]
    public class CierreDiarioCarteraInteresMoraSeven : BaseAsyncEndpoint
        .WithRequest<RequestCierreDiarioCarteraDto>
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
        public CierreDiarioCarteraInteresMoraSeven(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Cierre Diario Seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCuotaResponse</returns>
        [HttpPost("api/credito/CierreDiarioCarteraInteresMoraSeven")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Cierre Diario intereses mora  en Seven",
           Description = "Cierre Diario intereses mora en seven",
           OperationId = "Cartera.CierreDiarioCarteraInteresMoraSeven",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(RequestCierreDiarioCarteraDto request, CancellationToken cancellationToken = default)
        {
            var lis = new List<CierreDiarioCarteraDto>();
            return await this._creditoBusinessLogic.CierreDiarioCarteraInteresMoraSeven(request, cancellationToken, lis);
        }
    }
    
}
