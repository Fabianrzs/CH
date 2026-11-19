using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.SCnMcont;
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
    /// Cuenta contable 
    /// </summary>
    [Authorize]
    public class InsertarCnDmcon : BaseAsyncEndpoint
        .WithRequest<InsertarCnDmconDto>
        .WithResponse<Response<InsertarCnDmconResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public InsertarCnDmcon(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Obtener Cuenta Contable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/InsertarCnDmcon")]
        [ProducesResponseType(typeof(Response<InsertarCnDmconResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar Detalle Movimiento Contable",
            Description = "Insertar Detalle Movimiento Contable",
            OperationId = "Seven.InsertarCnDmcon",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<InsertarCnDmconResponse>>> HandleAsync(InsertarCnDmconDto request, CancellationToken cancellationToken = default)
        {
            return await _sevenBusinessLogic.InsertarCnDmconAsync(request, cancellationToken);
        }
    }
}
