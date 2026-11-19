using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSTercero;
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
    /// Endpoint para iniciar el proceso de pago o desembolso de cesantías con Seven.
    /// Paso 1 crear tercero en seven desde proceso de solicitud de cesantias.
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearTerceroSeven : BaseAsyncEndpoint
        .WithRequest<ParametrosObtenerCesantiasDto>
        .WithResponse<Response<InsertarGnTerceResponse>>
    {
        private readonly ICesantiasBusinessLogic _cesantiasBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cesantiasBusinessLogic"></param>
        public CrearTerceroSeven(ICesantiasBusinessLogic cesantiasBusinessLogic)
        {
            _cesantiasBusinessLogic = cesantiasBusinessLogic;
        }

        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de cesantias en seven primer paso crear el tercero en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cesantias/crearTerceroSeven")]
        [ProducesResponseType(typeof(Response<InsertarGnTerceResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Paso 1 crear tercero en seven",
            Description = "proceso de pago o desembolso de cesantías en seven",
            OperationId = "credito.crearTerceroSeven",
            Tags = new[] { "CesantiasEndpoint" })]
        public async override Task<ActionResult<Response<InsertarGnTerceResponse>>> HandleAsync(ParametrosObtenerCesantiasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cesantiasBusinessLogic.InsertTerceroSeven(request, cancellationToken);
        }
    }
}
