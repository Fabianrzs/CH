using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SevenEndpoints.Proveedores
{
    /// <summary>
    /// SyncFaConta
    /// </summary>
    [Authorize]
    public class SyncFaConta : BaseAsyncEndpoint
        .WithRequest<SyncFaContaDto>
        .WithResponse<Response<SyncFaContaResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public SyncFaConta(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/SyncFaConta")]
        [ProducesResponseType(typeof(Response<SyncFaContaResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Permite insertar o actualizar clientes.",
            Description = "Permite insertar o actualizar clientes.",
            OperationId = "Seven.SyncFaConta",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<SyncFaContaResponse>>> HandleAsync(SyncFaContaDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.SyncFaContaAsync(request, cancellationToken);
        }
    }
}
