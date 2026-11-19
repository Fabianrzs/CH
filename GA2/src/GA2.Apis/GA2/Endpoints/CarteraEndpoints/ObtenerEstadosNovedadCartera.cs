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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    [Authorize]

    public class ObtenerEstadosNovedadCartera : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<EstadosNovedadCarteraDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        public ObtenerEstadosNovedadCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/ObtenerEstadosNovedadCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<EstadosNovedadCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtener los estados de novedades de cartera",
           Description = "Obtener los estados de novedades de cartera",
           OperationId = "credito.ObtenerEstadosNovedadCartera",
           Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<EstadosNovedadCarteraDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ObtenerEstadosNovedadCartera();
        }
    }
}
