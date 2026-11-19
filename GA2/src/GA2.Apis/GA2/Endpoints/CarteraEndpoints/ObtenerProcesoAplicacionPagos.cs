using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Pagos;
using GA2.Application.Dto.Cartera.ParametersRequest;
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
    /// 
    [Authorize]
    public class ObtenerProcesoAplicacionPagos : BaseAsyncEndpoint.WithRequest<ParametrosObtenerProcesoAplicacionPagosDto>.WithResponse<Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerProcesoAplicacionPagos(ICarteraBusinessLogic carteraBusinessLogic)
        {
            _carteraBusinessLogic = carteraBusinessLogic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        /// 
        [HttpGet("api/credito/ObtenerProcesoAplicacionPagos")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Procesos Aplicacion Pagos",
            Description = "Obtener Procesos Aplicacion Pagos",
            OperationId = "credito.ObtenerProcesoAplicacionPagos",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>>> HandleAsync([FromQuery]ParametrosObtenerProcesoAplicacionPagosDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerProcesoAplicacionPagos(request);
        }
    }
}
