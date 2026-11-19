using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// Endpoint para iniciar el proceso de pago o desembolso de cesantías con Seven.
    /// </summary>
    /// <author>Gustavo Olarte</author>
    //[Authorize]
    public class PagosCesantiasSeven : BaseAsyncEndpoint
        .WithRequest<ParametrosObtenerCesantiasDto>
        .WithResponse<Response<PagosSevenDto>>
    {
        private readonly ICesantiasBusinessLogic _cesantiasBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cesantiasBusinessLogic"></param>
        public PagosCesantiasSeven(ICesantiasBusinessLogic cesantiasBusinessLogic)
        {
            _cesantiasBusinessLogic = cesantiasBusinessLogic;
        }

        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de cesantias en seven primer paso crear el tercero en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/cesantias/pagosCesantiasSeven")]
        [ProducesResponseType(typeof(Response<PagosSevenDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Proceso de desembolso soliciitudes cesantias en seven",
            Description = "Proceso de desembolso soliciitudes cesantias en seven",
            OperationId = "cesantias.pagosCesantiasSeven",
            Tags = new[] { "CesantiasEndpoint" })]
        public async override Task<ActionResult<Response<PagosSevenDto>>> HandleAsync(ParametrosObtenerCesantiasDto request, CancellationToken cancellationToken = default)
        {
            return await this._cesantiasBusinessLogic.PagosCesantiasSeven(request, cancellationToken);
        }
    }
}
