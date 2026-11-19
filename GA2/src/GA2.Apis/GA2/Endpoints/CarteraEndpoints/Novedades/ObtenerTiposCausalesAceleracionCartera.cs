using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Novedades;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints.Novedades
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ObtenerTiposCausalesAceleracionCartera :
        BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<TiposCausalesAceleracionCarteraDto>>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerTiposCausalesAceleracionCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Novedades/ObtenerTiposCausalesAceleracionCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<TiposCausalesAceleracionCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener los tipos de causales de caeleracion de cartera",
           Description = "Obtener los tipos de causales de caeleracion de cartera",
           OperationId = "credito.ObtenerTiposCausalesAceleracionCartera",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<TiposCausalesAceleracionCarteraDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerTiposCausalesAceleracionCartera();
        }
    }
}
