using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SevenEndpoints.MovimentosContables
{
    /// <summary>
    /// AnularCnMcont
    /// </summary>
    [Authorize]
    public class AnularCnMcont : BaseAsyncEndpoint
        .WithRequest<AnularCnMcontDto>
        .WithResponse<Response<AnularCnMcontResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public AnularCnMcont(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Obtener Cuenta Contable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/AnularCnMcont")]
        [ProducesResponseType(typeof(Response<AnularCnMcontResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Anular Movimiento Contable",
            Description = "Anular Movimiento Contable",
            OperationId = "Seven.AnularCnMcont",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<AnularCnMcontResponse>>> HandleAsync(AnularCnMcontDto request, CancellationToken cancellationToken = default)
        {
            return await _sevenBusinessLogic.AnularCnMcontAsync(request, cancellationToken);
        }
    }
}
