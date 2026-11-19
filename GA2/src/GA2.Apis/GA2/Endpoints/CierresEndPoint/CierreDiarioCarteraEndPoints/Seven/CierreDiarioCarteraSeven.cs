using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using GA2.Application.Dto.Cartera.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Application.Dto.Cartera.Cierres.RequestCierreDiarioCartera;

namespace GA2.Apis.GA2.Endpoints.CierresEndPoint.CierreDiarioCarteraEndPoints.Seven
{
    public class CierreDiarioCarteraSeven : BaseAsyncEndpoint
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
        public CierreDiarioCarteraSeven(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Cierre Diario Seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCuotaResponse</returns>
        [HttpPost("api/credito/CierreDiarioCarteraSeven")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Cierre Diario de los diferentes abonos en Seven",
           Description = "Cierre Diario en seven",
           OperationId = "Cartera.CierreDiarioCarteraSeven",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(CierreDiarioDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CierreDiarioCarteraSeven(request, cancellationToken);
        }
    }
}
