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
    /// Crea el cdp
    /// </summary>
    [Authorize]
    public class InsertarCDP : BaseAsyncEndpoint
        .WithRequest<InsertarCDPDto>
        .WithResponse<Response<InsertarCDPResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public InsertarCDP(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// InsertarCDP
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/InsertarCDP")]
        [ProducesResponseType(typeof(Response<InsertarCDPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear CDP",
            Description = "Crear CDP",
            OperationId = "Seven.InsertarCDP",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<InsertarCDPResponse>>> HandleAsync(InsertarCDPDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.InsertarCDPAsync(request, cancellationToken);
        }
    }
}
