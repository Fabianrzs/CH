using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSWintipro;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SevenEndpoints.CuentasContables
{
    /// <summary>
    /// Cuenta contable 
    /// </summary>
    [Authorize]
    public class GetCuentaContable : BaseAsyncEndpoint
        .WithRequest<CuentaContableDto>
        .WithResponse<Response<GetCuentaContableResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public GetCuentaContable(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Obtener Cuenta Contable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/GetCuentaContable")]
        [ProducesResponseType(typeof(Response<GetCuentaContableResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Cuenta Contable",
            Description = "Obtener Cuenta Contable",
            OperationId = "Seven.GetCuentaContable",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<GetCuentaContableResponse>>> HandleAsync(CuentaContableDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.GetCuentaContableAsync(request, cancellationToken);
        }
    }
}
