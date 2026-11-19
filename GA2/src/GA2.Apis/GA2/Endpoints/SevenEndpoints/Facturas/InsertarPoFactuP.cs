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
    /// InsertarPoFactuP
    public class InsertarPoFactuP : BaseAsyncEndpoint
        .WithRequest<InsertarPoFactuPDto>
        .WithResponse<Response<InsertarPoFactuPResponse>>
    {
        private readonly ISevenBusinessLogic _sevenBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sevenBusinessLogic"></param>
        public InsertarPoFactuP(ISevenBusinessLogic sevenBusinessLogic)
        {
            _sevenBusinessLogic = sevenBusinessLogic;
        }

        /// <summary>
        /// Crear Cdp
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/InsertarPoFactuP")]
        [ProducesResponseType(typeof(Response<InsertarPoFactuPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Insertar Factura",
            Description = "Insertar Factura",
            OperationId = "Seven.InsertarPoFactuP",
            Tags = new[] { "Seven" })]
        public async override Task<ActionResult<Response<InsertarPoFactuPResponse>>> HandleAsync(InsertarPoFactuPDto request, CancellationToken cancellationToken = default)
        {
            return await this._sevenBusinessLogic.InsertarPoFactuPAsync(request, cancellationToken);
        }
    }
}
