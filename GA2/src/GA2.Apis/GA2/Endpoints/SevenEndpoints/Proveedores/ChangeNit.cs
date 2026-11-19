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
    /// CambiarNit
    /// </summary>
    [Authorize]
    public class ChangeNit : BaseAsyncEndpoint
        .WithRequest<ChangeNitDto>
        .WithResponse<Response<ChangeNitResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public ChangeNit(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ChangeNit")]
        [ProducesResponseType(typeof(Response<ChangeNitResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Change Nit",
            Description = "Change Nit",
            OperationId = "Seven.ChangeNit",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<ChangeNitResponse>>> HandleAsync(ChangeNitDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.InsertarCnMcontAsync(request, cancellationToken);
        }
    }
}
