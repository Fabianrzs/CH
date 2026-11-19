using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SevenEndpoints.CDP
{
    /// <summary>
    /// Anular CDP Endpoint
    /// </summary>
    [Authorize]
    public class AplicarCDP : BaseAsyncEndpoint
        .WithRequest<AplicarCDPDto>
        .WithResponse<Response<AplicarCDPResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public AplicarCDP(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/AplicarCDP")]
        [ProducesResponseType(typeof(Response<AplicarCDPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Aplicar CDP",
            Description = "Aplicar CDP",
            OperationId = "Seven.AplicarCDP",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<AplicarCDPResponse>>> HandleAsync(AplicarCDPDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.AplicarCDPAsync(request, cancellationToken);
        }
    }
}
