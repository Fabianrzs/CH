using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SevenEndpoints.CDP
{
    [Authorize]
    /// <summary>
    /// Anular CDP Endpoint
    /// </summary>
    /// RevertirMovimientoContable
    public class AnularFactura : BaseAsyncEndpoint
        .WithRequest<AnularFacturaDto>
        .WithResponse<Response<AnularFacturaResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public AnularFactura(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/AnularFactura")]
        [ProducesResponseType(typeof(Response<AnularFacturaResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Anular Factura",
            Description = "Anular Factura",
            OperationId = "Seven.AnularFactura",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<AnularFacturaResponse>>> HandleAsync(AnularFacturaDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.AnularFacturaAsync(request, cancellationToken);
        }
    }
}
