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
using GA2.Application.Dto.Cartera.Cierres.CierresCausacionesRealizadas;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CierresEndPoint.CierresCausacionesRealizadasEndPoints
{
    /// <summary>
    /// CierresCausacionesRealizadas
    /// </summary>
    [Authorize]
    public class CierresCausacionesRealizadas : BaseAsyncEndpoint
        .WithRequest<RequestCierresCausacionesRealizadasDto>
        .WithResponse<Response<IEnumerable<CierresCausacionesRealizadasDto>>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public CierresCausacionesRealizadas(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ObtenerCierresCausacionesRealizadas")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<CierresCausacionesRealizadasDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene los cierres y causaciones realizadas",
           Description = "Obtiene los cierres y causaciones realizada en un rango de fechas determinados y de los tipos especificados",
           OperationId = "Credito.ObtenerCierresCausacionesRealizadas",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CierresCausacionesRealizadasDto>>>> HandleAsync(RequestCierresCausacionesRealizadasDto request, CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerCierresCausacionesRealizadas(request);
        }
    }
}
