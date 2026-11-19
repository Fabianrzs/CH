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
    public class ActualizarNovedadCartera : BaseAsyncEndpoint.WithRequest<NovedadCarteraDto>.WithResponse<Response<NovedadCarteraDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ActualizarNovedadCartera(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Novedades/ActualizarNovedadCartera")]
        [ProducesResponseType(typeof(Response<NovedadCarteraDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Actualiza una novedad de cartera",
            Description = "Actualiza una novedad de cartera",
            OperationId = "credito.ActualizarNovedadCartera",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<NovedadCarteraDto>>> HandleAsync(NovedadCarteraDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ActualizarNovedadCartera(request);
        }
    }
}
