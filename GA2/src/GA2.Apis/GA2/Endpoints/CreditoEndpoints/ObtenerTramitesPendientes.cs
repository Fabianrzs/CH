using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
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
    /// ObtenerTramitesPendientes
    /// </summary>
    [Authorize]
    public class ObtenerTramitesPendientes : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response<IEnumerable<TramitesPendientesDto>>>
    {
        private readonly ICreditoBusinessLogic _estadosolicitudBusinessLogic;

        /// <summary>
        /// ObtenerTramitesPendientes
        /// </summary>
        /// <param name="estadosolicitudBusinessLogic"></param>
        public ObtenerTramitesPendientes(ICreditoBusinessLogic estadosolicitudBusinessLogic)
        {
            _estadosolicitudBusinessLogic = estadosolicitudBusinessLogic;
        }


        /// <summary>
        /// Obtiene all ciudades
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/credito/ObtenerTramitesPendientes")]
        [ProducesResponseType(typeof(Response<IEnumerable<TramitesPendientesDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtiene Tramites Pendientes",
            Description = "Obtiene Tramites Pendientes",
            OperationId = "credito.ObtenerTramitesPendientes",
            Tags = new[] { "CreditoEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<TramitesPendientesDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._estadosolicitudBusinessLogic.ObtenerTramitesPendientes();
        }

    }
}
