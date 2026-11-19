using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSAportes;
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
    public class GetAporte : BaseAsyncEndpoint
        .WithRequest<ValidarAportesDto>
        .WithResponse<Response<GetAporteResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public GetAporte(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/GetAporte")]
        [ProducesResponseType(typeof(Response<GetAporteResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Validar Aportes",
            Description = "Validar Aportes",
            OperationId = "Seven.GetAporte",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<GetAporteResponse>>> HandleAsync(ValidarAportesDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.GetAporteAsync(request, cancellationToken);
        }
    }
}
