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
    public class InsertarMovContable : BaseAsyncEndpoint
        .WithRequest<InsertarMovContableDto>
        .WithResponse<Response<InsertarMovContableResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public InsertarMovContable(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Obtener Cuenta Contable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/InsertarMovContable")]
        [ProducesResponseType(typeof(Response<InsertarMovContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar Encabezado Movimiento Contable",
            Description = "Insertar Encabezado Movimiento Contable",
            OperationId = "Seven.InsertarMovContable",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<InsertarMovContableResponse>>> HandleAsync(InsertarMovContableDto request, CancellationToken cancellationToken = default)
        {
            return await _sevenBusinessLogic.InsertarMovContableAsync(request, cancellationToken);
        }
    }
}
