using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Application.Dto.Cartera.Cierres.RequestCierreDiarioCartera;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Transversals.Commons;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Authorization;

namespace GA2.Apis.GA2.Endpoints.CierresEndPoint.CierreDiarioCarteraEndPoints.Seven
{
    /// <summary>
    /// CierreDiarioCartera
    /// </summary>
    [Authorize]
    public class CierreCausacionDiariaInteresesCorrientesSeven : BaseAsyncEndpoint
        .WithRequest<RequestCausacionDiariaInteresesCorrientesDto>
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
        public CierreCausacionDiariaInteresesCorrientesSeven(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Cierre Diario Seven 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>CrearCuotaResponse</returns>
        [HttpPost("api/credito/CierreDiarioCausacionDiariaInteresesCorrientesSeven")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Cierre Diario de la causacion de interes corriente en Seven",
           Description = "Cierre Diario de la causacion de interes corriente en seven",
           OperationId = "Cartera.CierreDiarioCausacionDiariaInteresesCorrientesSeven",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(RequestCausacionDiariaInteresesCorrientesDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.CierreDiarioCausacionDiariaInteresesCorrientesSeven(request);
        }
    }
}
