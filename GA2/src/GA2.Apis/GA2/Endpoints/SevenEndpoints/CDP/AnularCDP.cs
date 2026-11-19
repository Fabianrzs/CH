using Ardalis.ApiEndpoints;
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
    public class AnularCDP : BaseAsyncEndpoint
            .WithRequest<AnularCDPDto>
            .WithResponse<Response<AnularCDPResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public AnularCDP(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/AnularCDP")]
        [ProducesResponseType(typeof(Response<AnularCDPResponse>), StatusCodes.Status200OK)]
        [Consumes("application/json")]
        [SwaggerOperation(
            Summary = "Anular CDP",
            Description = "Anular CDP",
            OperationId = "Seven.AnularCDP",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<AnularCDPResponse>>> HandleAsync([FromBody] AnularCDPDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.AnularCDPAsync(request, cancellationToken);
        }
    }
}
