using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
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
    /// Endpoint para consultar usuarios de crédito.
    /// </summary>
    /// <author>Alejandro Lara</author>
    /// <date>24/07/2026</date>
    public class ConsultarUsuarioCredito :
         BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response<IEnumerable<UsuarioCreditoTramite>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ConsultarUsuarioCredito(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Consulta los usuarios de crédito.
        /// </summary>
        /// <param name="request">1 = Perito, 2 = Abogado</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/consultarusuariocredito/{request}")]
        [ProducesResponseType(typeof(Response<IEnumerable<UsuarioCreditoTramite>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar Usuario Crédito",
            Description = "Consulta los usuarios de crédito por indicador.",
            OperationId = "Credito.consultarusuariocredito",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<UsuarioCreditoTramite>>>> HandleAsync(
            [FromRoute] int request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ConsultarUsuarioCredito(request);
        }
    }
}