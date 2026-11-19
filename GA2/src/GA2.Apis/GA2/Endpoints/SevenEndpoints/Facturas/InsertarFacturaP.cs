using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Seven;
using GA2.Domain.Core.Credito.Seven;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.SevenEndpoints.Facturas
{
    [Authorize]
    /// <summary>
    /// Insertar FacturaP Endpoint
    /// </summary>
    /// InsertarFacturaP
    public class InsertarFacturaP : BaseAsyncEndpoint
        .WithRequest<InsertarFacturaPDto>
        .WithResponse<Response<InsertarFacturaPResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public InsertarFacturaP(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/InsertarFacturaP")]
        [ProducesResponseType(typeof(Response<InsertarFacturaPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar Factura",
            Description = "Insertar Factura",
            OperationId = "Seven.InsertarFacturaP",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<InsertarFacturaPResponse>>> HandleAsync(InsertarFacturaPDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.InsertarFacturaPAsync(request, cancellationToken);
        }
    }
}
