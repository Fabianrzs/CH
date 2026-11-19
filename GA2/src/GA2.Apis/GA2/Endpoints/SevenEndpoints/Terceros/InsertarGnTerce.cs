using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
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
    public class InsertarGnTerce : BaseAsyncEndpoint
        .WithRequest<InsertarTerceroDto>
        .WithResponse<Response<InsertarGnTerceResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public InsertarGnTerce(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/InsertarGnTerce")]
        [ProducesResponseType(typeof(Response<InsertarGnTerceResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar Tercero",
            Description = "Insertar Tercero",
            OperationId = "Seven.InsertarGnTerce",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<InsertarGnTerceResponse>>> HandleAsync(InsertarTerceroDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.InsertarGnTerceAsync(request, cancellationToken);
        }
    }
}
