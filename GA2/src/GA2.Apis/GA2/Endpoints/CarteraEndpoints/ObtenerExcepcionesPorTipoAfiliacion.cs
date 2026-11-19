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

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ObtenerExcepcionesPorTipoAfiliacion : BaseAsyncEndpoint.WithRequest<int>.WithResponse<Response<ExcepcionTasaActorDto>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        public ObtenerExcepcionesPorTipoAfiliacion(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cartera/ObtenerExcepcionesPorTipoAfiliacion")]
        [ProducesResponseType(typeof(Response<ExcepcionTasaActorDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Excepciones Por Tipo Afiliacion",
            Description = "Obtener Excepciones Por Tipo Afiliacion",
            OperationId = "cartera.ObtenerExcepcionesPorTipoAfiliacion",
            Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<ExcepcionTasaActorDto>>> HandleAsync(int request, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ObtenerExcepcionesPorTipoAfiliacion(request);
        }
    }
    }
