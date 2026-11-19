using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.Cierres.CierreAnual;
using GA2.Domain.Core;
using GA2.Domain.Entities.Cartera.Cierres.CierreAnual;
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

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// CierreAnualCartera
    /// </summary>
    [Authorize]
    public class CierreAnualCartera : BaseAsyncEndpoint
        .WithRequest<RequestCierreAnualCartera>
        .WithResponse<Response<IEnumerable<ReporteCierreAnualCarteraDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        ///  
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CierreAnualCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// CierreAnualCartera
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/CierreAnualCartera")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteCierreMensualCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Ejecutar Cierre Anual Cartera",
           Description = "Ejecutar Cierre Anual Cartera",
           OperationId = "Credito.CierreAnualCartera",
           Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<ReporteCierreAnualCarteraDto>>>> HandleAsync(RequestCierreAnualCartera request, CancellationToken cancellationToken = default)
        {
            //throw new Exception("Error");
            request.CreadoPor = Guid.Parse(User.Claims.Where(c => c.Type == "USR_ID").FirstOrDefault().Value);
            return await _carteraBusinessLogic.CierreAnualCartera(request);
        }
    }
}