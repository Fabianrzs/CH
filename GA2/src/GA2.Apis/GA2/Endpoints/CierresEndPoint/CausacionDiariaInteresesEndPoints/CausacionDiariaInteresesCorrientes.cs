using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Cierres.CausacionDiariaInteresesCorrientes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CierresEndPoint.CausacionDiariaInteresesEndPoints
{
    /// <summary>
    /// CausacionDiariaInteresesCorrientes
    /// </summary>
    [Authorize]
    public class CausacionDiariaInteresesCorrientes : BaseAsyncEndpoint
        .WithRequest<RequestCausacionDiariaInteresesCorrientesDto>
        .WithResponse<Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CausacionDiariaInteresesCorrientes(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CausacionDiariaInteresesCorrientes")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Ejecutar causación diaria de intereses corrientes",
           Description = "Ejecutar causación diaria de intereses corrientes",
           OperationId = "Credito.CausacionDiariaInteresesCorrientes",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CausacionDiariaInteresesCorrientesDto>>>> HandleAsync(RequestCausacionDiariaInteresesCorrientesDto request, CancellationToken cancellationToken = default)
        {
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await this._creditoBusinessLogic.CausacionDiariaInteresesCorrientes(request);
        }
    }
}
