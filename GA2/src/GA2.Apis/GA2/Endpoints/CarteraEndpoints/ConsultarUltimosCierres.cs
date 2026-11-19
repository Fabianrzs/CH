using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Cierres;
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
    /// Endpoint que permiten consultar últimos cierres cartera realizados
    /// </summary>
    [Authorize]
    public class ConsultarUltimosCierres : BaseAsyncEndpoint.WithoutRequest.WithResponse<Response<IEnumerable<UltimoCierreDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;


        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>

        public ConsultarUltimosCierres(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// Permite consultar últimos cierres cartera
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/cartera/ConsultarUltimosCierres")]
        [ProducesResponseType(typeof(Response<IEnumerable<UltimoCierreDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Consultar ultimos cierres",
            Description = "Consulta los últimos cierres por tipo",
            OperationId = "cartera.ConsultarUltimosCierres",
            Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<UltimoCierreDto>>>> HandleAsync(CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ConsultarUltimosCierres();
        }
    }
}
