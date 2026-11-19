using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class ObtenerRestitucionInmuebleCartera : BaseAsyncEndpoint.WithRequest<Guid>
        .WithResponse<Response<IEnumerable<RestitucionInmuebleCarteraDto>>>
    {


        private readonly ICarteraBusinessLogic _carteraBusinessLogic;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerRestitucionInmuebleCartera(ICarteraBusinessLogic carteraBusinessLogic)
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
        [HttpGet("api/cartera/ObtenerRestitucionInmuebleCartera")]
        [ProducesResponseType(typeof(Response<IEnumerable<RestitucionInmuebleCarteraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener la restitucion de inmueble de cartera",
            Description = "Obtener la restitucion de inmueble de cartera",
            OperationId = "credito.ObtenerRestitucionInmuebleCartera",
            Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<RestitucionInmuebleCarteraDto>>>> HandleAsync(Guid request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ObtenerRestitucionInmuebleCartera(request);
        }
    }
}
