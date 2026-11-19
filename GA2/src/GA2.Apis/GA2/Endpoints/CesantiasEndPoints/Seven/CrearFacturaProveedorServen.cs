using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSFactura;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSProveedor;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Paso 4 Creación Factura Proveedor Seven desde solic. cesantías
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearFacturaProveedorServen : BaseAsyncEndpoint
        .WithRequest<ParametrosObtenerCesantiasDto>
        .WithResponse<Response<InsertarFacturaPResponse>>
    {
        private readonly ICesantiasBusinessLogic _cesantiasBusinessLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cesantiasBusinessLogic"></param>
        public CrearFacturaProveedorServen(ICesantiasBusinessLogic cesantiasBusinessLogic)
        {
            _cesantiasBusinessLogic = cesantiasBusinessLogic;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/cesantias/crearFacturaProveSeven")]
        [ProducesResponseType(typeof(Response<InsertarFacturaPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Paso 4 crear factura proveedor en seven",
            Description = "proceso de pago o desembolso de cesantías en seven",
            OperationId = "credito.crearFacturaProveedorSeven",
            Tags = new[] { "CesantiasEndpoint" })]
        public async override Task<ActionResult<Response<InsertarFacturaPResponse>>> HandleAsync(ParametrosObtenerCesantiasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cesantiasBusinessLogic.CrearFacturaProveSeven(request, cancellationToken);
        }
    }
}
