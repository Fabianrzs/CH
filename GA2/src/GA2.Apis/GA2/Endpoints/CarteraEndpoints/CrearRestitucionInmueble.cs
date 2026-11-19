using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class CrearRestitucionInmueble : BaseAsyncEndpoint.WithRequest<RestitucionInmuebleCarteraDto>
        .WithResponse<Response<Guid>>
    {


        private readonly ICarteraBusinessLogic _carteraBusinessLogic;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public CrearRestitucionInmueble(ICarteraBusinessLogic carteraBusinessLogic)
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
        [HttpPost("api/cartera/CrearRestitucionInmueble")]
        [ProducesResponseType(typeof(Response<Guid>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Crear la restitucion de inmueble de cartera",
            Description = "Crear la restitucion de inmueble de cartera",
            OperationId = "credito.CrearRestitucionInmueble",
            Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<Guid>>> HandleAsync(RestitucionInmuebleCarteraDto request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.CrearRestitucionInmueble(request);
        }
    }
}
