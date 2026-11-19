using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
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
    /// EmpleadoProveedor
    /// </summary>
    [Authorize]
    public class SyncEmpleadoPvdor : BaseAsyncEndpoint
        .WithRequest<SyncEmpleadoPvdorDto>
        .WithResponse<Response<SyncEmpleadoPvdorResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public SyncEmpleadoPvdor(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/SyncEmpleadoPvdor")]
        [ProducesResponseType(typeof(Response<SyncEmpleadoPvdorResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Permite insertar o actualizar proveedores que provienen de Kactus",
            Description = "Permite insertar o actualizar proveedores que provienen de Kactus",
            OperationId = "Seven.SyncEmpleadoPvdor",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<SyncEmpleadoPvdorResponse>>> HandleAsync(SyncEmpleadoPvdorDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.SyncEmpleadoPvdorAsync(request, cancellationToken);
        }
    }
}
