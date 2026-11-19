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
    /// AplicarCnMcont
    /// </summary>
    [Authorize]
    public class AplicarCnMcont : BaseAsyncEndpoint
        .WithRequest<AplicarCnMcontDto>
        .WithResponse<Response<AplicarCnMcontResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public AplicarCnMcont(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Obtener Cuenta Contable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/AplicarCnMcont")]
        [ProducesResponseType(typeof(Response<AplicarCnMcontResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Aplicar Movimiento Contable",
            Description = "Aplicar Movimiento Contable",
            OperationId = "Seven.AplicarCnMcont",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<AplicarCnMcontResponse>>> HandleAsync(AplicarCnMcontDto request, CancellationToken cancellationToken = default)
        {
            return await _sevenBusinessLogic.AplicarCnMcontAsync(request, cancellationToken);
        }
    }
}
