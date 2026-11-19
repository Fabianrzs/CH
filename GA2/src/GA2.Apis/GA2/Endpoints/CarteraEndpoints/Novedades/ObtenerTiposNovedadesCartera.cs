using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Novedades
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ObtenerTiposNovedadesCartera :
        BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<TiposNovedadesCarteraDto>>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerTiposNovedadesCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Novedades/ObtenerTiposNovedadesCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<TiposNovedadesCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener los tipos de novedades cde cartera",
           Description = "Obtener los tipos de novedades cde cartera",
           OperationId = "credito.ObtenerTiposNovedadesCartera",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<TiposNovedadesCarteraDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerTiposNovedadesCartera();
        }
    }
}
