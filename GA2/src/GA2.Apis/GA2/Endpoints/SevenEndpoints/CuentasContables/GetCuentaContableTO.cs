using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
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
    public class GetCuentaContableTO : BaseAsyncEndpoint
        .WithRequest<CuentaContableToDto>
        .WithResponse<Response<GetCuentaContableTOResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public GetCuentaContableTO(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Obtener Cuenta Contable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/GetCuentaContableTO")]
        [ProducesResponseType(typeof(Response<GetCuentaContableTOResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Cuenta Contable",
            Description = "Obtener Cuenta Contable",
            OperationId = "Seven.GetCuentaContableTO",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<GetCuentaContableTOResponse>>> HandleAsync(CuentaContableToDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.GetCuentaContableTOAsync(request, cancellationToken);
        }
    }
}
