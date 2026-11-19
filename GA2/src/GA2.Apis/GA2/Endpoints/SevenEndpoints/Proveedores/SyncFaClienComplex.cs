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
    /// SyncFaClienComplex
    /// </summary>
    [Authorize]
    public class SyncFaClienComplex : BaseAsyncEndpoint
        .WithRequest<SyncFaClienComplexDto>
        .WithResponse<Response<SyncFaClienComplexResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public SyncFaClienComplex(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/SyncFaClienComplex")]
        [ProducesResponseType(typeof(Response<SyncFaClienComplexResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Permite insertar o actualizar clientes objeto complejo SEVEN - ERP.",
            Description = "Permite insertar o actualizar clientes objeto complejo SEVEN - ERP.",
            OperationId = "Seven.SyncFaClienComplex",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<SyncFaClienComplexResponse>>> HandleAsync(SyncFaClienComplexDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.SyncFaClienComplexAsync(request, cancellationToken);
        }
    }
}
