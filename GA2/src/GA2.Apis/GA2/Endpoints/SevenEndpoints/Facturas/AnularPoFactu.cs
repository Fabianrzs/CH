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
    /// 
    /// </summary>
    public class AnularPoFactu : BaseAsyncEndpoint
        .WithRequest<AnularPoFactuDto>
        .WithResponse<Response<AnularPoFactuResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public AnularPoFactu(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/AnularPoFactu")]
        [ProducesResponseType(typeof(Response<AnularPoFactuResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Anular Factura",
            Description = "Anular Factura",
            OperationId = "Seven.AnularPoFactu",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<AnularPoFactuResponse>>> HandleAsync(AnularPoFactuDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.AnularPoFactuAsync(request, cancellationToken);
        }
    }
}
