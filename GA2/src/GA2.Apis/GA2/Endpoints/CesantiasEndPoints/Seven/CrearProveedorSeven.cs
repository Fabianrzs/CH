using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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
    /// Paso 3 Creación Pago Proveedor seven desde solic. cesantías
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearProveedorSeven : BaseAsyncEndpoint
        .WithRequest<ParametrosObtenerCesantiasDto>
        .WithResponse<Response<SyncPoPvdorResponse>>
    {
        private readonly ICesantiasBusinessLogic _cesantiasBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cesantiasBusinessLogic"></param>
        public CrearProveedorSeven(ICesantiasBusinessLogic cesantiasBusinessLogic)
        {
            _cesantiasBusinessLogic = cesantiasBusinessLogic;
        }
        /// <summary>
        /// Endpoint paso 3 integración Seven con Cesantías creación de Proveedor.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cesantias/crearProveedorSeven")]
        [ProducesResponseType(typeof(Response<SyncPoPvdorResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Paso 3 crear proveedor en seven",
            Description = "proceso de pago o desembolso de cesantías en seven",
            OperationId = "credito.crearProveedorSeven",
            Tags = new[] { "CesantiasEndpoint" })]
        public async override Task<ActionResult<Response<SyncPoPvdorResponse>>> HandleAsync(ParametrosObtenerCesantiasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cesantiasBusinessLogic.CrearProveedorSeven(request, cancellationToken);
        }

    }
}
