using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ObtenerIPC
    /// </summary>
    [Authorize]
    public class ObtenerIPC : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IPCDto>>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ICierreMensualCuentasBusinessLogic _cierreMensualBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cierreMensualBusinessLogic"></param>
        public ObtenerIPC(ICierreMensualCuentasBusinessLogic cierreMensualBusinessLogic)
        {
            _cierreMensualBusinessLogic = cierreMensualBusinessLogic;
        }


        [HttpGet("api/ObtenerIPC")]
        [ProducesResponseType(typeof(Response<IPCDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene el IPC del mes inmediatamente anterior",
            Description = "Obtiene el IPC del mes inmediatamente anterior",
            OperationId = "CierreMensual.ObtenerIPC",
            Tags = new[] { "ObtenerIPCEndPoint" })]
        public override async Task<ActionResult<Response<IPCDto>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._cierreMensualBusinessLogic.ObtenerIPCMes();
        }
    }
}
