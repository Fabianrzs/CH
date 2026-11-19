using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ObtenerCuotasAdelantadas : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<CuotaAdelantadaDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerCuotasAdelantadas(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCredito"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/cartera/ObtenerCuotasAdelantadas")]
        [ProducesResponseType(typeof(Response<IEnumerable<AplicacionPagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener cuotas adelantadas",
           Description = "Obtener cuotas adelantadas",
           OperationId = "cartera.ObtenerCuotasAdelantadas",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<CuotaAdelantadaDto>>>> HandleAsync(Guid idCredito, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerCuotasAdelantadas(idCredito);
        }
    }

}
