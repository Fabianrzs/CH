using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera;
using GA2.Application.Dto;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using GA2.Transversals.Commons;
using System.Collections.Generic;

namespace GA2.Apis.GA2.Endpoints.CarteraEndpoints
{
    public class ObtenerEstadoAbonoCredito : BaseAsyncEndpoint.WithRequest<ObtenerProductoCartera>.WithResponse<Response<List<AbonoHistoricoDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerEstadoAbonoCredito(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/obtenerEstadoAbonoCredito")]
        [ProducesResponseType(typeof(Response<List<AbonoHistoricoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
           Summary = "Obtiene el estado de abono de credito de cartera",
           Description = "Obtiene el estado de abono de credito de cartera",
           OperationId = "credito.obtenerEstadoAbonoCredito",
           Tags = new[] { "CarteraEndpoint" })]
        public async override Task<ActionResult<Response<List<AbonoHistoricoDto>>>> HandleAsync(ObtenerProductoCartera obtenerProductoCartera, CancellationToken cancellationToken = default)
        {
            return await this._carteraBusinessLogic.ObtenerEstadoCuotasCredito(obtenerProductoCartera.Nro_Credito.ToString(), obtenerProductoCartera.Tcr_Id);
        }
    }
}
