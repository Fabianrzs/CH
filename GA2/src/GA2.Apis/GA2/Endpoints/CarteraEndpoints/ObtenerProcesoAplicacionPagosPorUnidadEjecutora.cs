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
    public class ObtenerProcesoAplicacionPagosPorUnidadEjecutora : BaseAsyncEndpoint.WithRequest<ParametrosObtenerProcesoAplicacionPagosPorUEJDto>.WithResponse<Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>>
    {
        private readonly ICarteraBusinessLogic _carteraBusinessLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="carteraBusinessLogic"></param>
        public ObtenerProcesoAplicacionPagosPorUnidadEjecutora(ICarteraBusinessLogic carteraBusinessLogic)
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
        [HttpGet("api/credito/ObtenerProcesoAplicacionPagosPorUnidadEjecutora")]
        [ProducesResponseType(typeof(Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener procesos aplicacion pagos para una unidad ejecutora",
            Description = "Obtener procesos aplicacion pagos para una unidad ejecutora",
            OperationId = "credito.ObtenerProcesoAplicacionPagosPorUnidadEjecutora",
            Tags = new[] { "CarteraEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ProcesoAplicacionPagosMasivoDto>>>> HandleAsync([FromQuery] ParametrosObtenerProcesoAplicacionPagosPorUEJDto request, CancellationToken cancellationToken = default)
        {
            return await _carteraBusinessLogic.ObtenerProcesoAplicacionPagos(new()
            {
                IdUnidadEjecutora = request.UnidadEjecutora,
                IdTipoCredito = request.TipoCredito,
                Periodo = request.Periodo,
                Estado = request.Estado
            });
        }
    }
}
