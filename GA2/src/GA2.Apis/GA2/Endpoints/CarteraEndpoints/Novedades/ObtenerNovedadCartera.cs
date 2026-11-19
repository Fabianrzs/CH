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
    public class ObtenerNovedadesCartera : BaseAsyncEndpoint.WithRequest<Guid>.WithResponse<Response<IEnumerable<NovedadCarteraDto>>>
    {

        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerNovedadesCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Novedades/ObtenerNovedadesCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<NovedadCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener las novedades de cartera de un credito",
           Description = "Obtener las novedades de cartera de un credito",
           OperationId = "credito.ObtenerNovedadesCartera",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<NovedadCarteraDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerNovedadesCartera(request);
        }
    }
}
