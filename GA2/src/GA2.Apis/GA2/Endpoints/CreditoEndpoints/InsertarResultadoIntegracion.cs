using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]
    public class InsertarResultadoIntegracion :BaseAsyncEndpoint.WithRequest<ResultadoIntegracionDto>.WithResponse<Response<ResultadoIntegracionDto>>
    {
        ICreditoBusinessLogic _creditoBusinessLogic;

        public InsertarResultadoIntegracion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/InsertarResultadoIntegracion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<ResultadoIntegracionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Inserta Resultado Integracion fenix y vigia",
           Description = "Inserta Resultado Integracion fenix y vigia",
           OperationId = "Credito.InsertarResultadoIntegracion",
           Tags = new[] { "CreditoEndpoint" })]
     
        public override async Task<ActionResult<Response<ResultadoIntegracionDto>>> HandleAsync(ResultadoIntegracionDto request, CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.InsertarResultadoIntegracion(request);
        }
    }
}
