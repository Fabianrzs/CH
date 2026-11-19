using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Seven.WSCDP;
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
    /// Paso 2 crear CDP en seven desde proceso de solicitud de cesantias.
    /// </summary>
    /// <author>Gustavo Olarte</author>
    [Authorize]
    public class CrearCDPSeven : BaseAsyncEndpoint
        .WithRequest<ParametrosObtenerCesantiasDto>
        .WithResponse<Response<CrearCDPResponse>>
    {
        private readonly ICesantiasBusinessLogic _cesantiasBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cesantiasBusinessLogic"></param>
        public CrearCDPSeven(ICesantiasBusinessLogic cesantiasBusinessLogic)
        {
            _cesantiasBusinessLogic = cesantiasBusinessLogic;
        }
        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de cesantias en seven paso 2 crear el CDP en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cesantias/crearCDPSeven")]
        [ProducesResponseType(typeof(Response<CrearCDPResponse>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Paso 2 crear cdp en seven",
            Description = "proceso de pago o desembolso de cesantías en seven",
            OperationId = "credito.crearCDPSeven",
            Tags = new[] { "CesantiasEndpoint" })]
        public async override Task<ActionResult<Response<CrearCDPResponse>>> HandleAsync(ParametrosObtenerCesantiasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cesantiasBusinessLogic.CrearCDPSeven(request, cancellationToken);
        }
    }
}
