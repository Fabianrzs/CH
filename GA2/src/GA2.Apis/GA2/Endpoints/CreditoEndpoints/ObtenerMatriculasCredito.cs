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

namespace GA2.Apis.Identity.EndPoints
{
    /// <summary>
    /// Endpoint para Obtener MatriculasCredito
    /// </summary>
    /// <author>Nelson Redondo Barros</author>
    /// <date>2/07/2023</date>
    [Authorize]
        public class ObtenerMatriculasCredito : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<MatriculaCreditoDto>>>
        {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        public ObtenerMatriculasCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        [HttpGet("api/credito/ObtenerMatriculasCredito")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<MatriculaCreditoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener Matricula Credito",
           Description = "Obtener Matricula Credito",
           OperationId = "Credito.ObtenerMatriculasCredito",
           Tags = new[] { "CreditoEndpoint" })]
      
        public override async Task<ActionResult<Response<IEnumerable<MatriculaCreditoDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._creditoBusinessLogic.ObtenerMatriculasCredito();
        }
    }
}