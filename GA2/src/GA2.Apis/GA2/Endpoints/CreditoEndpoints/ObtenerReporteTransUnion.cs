using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Credito;
using GA2.Domain.Core;
using GA2.Domain.Entities.Credito;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.CreditoEndpoints
{
    public class ObtenerReporteTransUnion :
        BaseAsyncEndpoint.WithRequest<ReporteTransUnionDto>
        .WithResponse<Response<IEnumerable<ReporteProspeccionPreaprobado>>>
    {
        private readonly ICreditoBusinessLogic _creditoBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="creditoBusinessLogic"></param>
        public ObtenerReporteTransUnion(ICreditoBusinessLogic creditoBusinessLogic)
        {
            _creditoBusinessLogic = creditoBusinessLogic;
        }

        /// <summary>
        /// Obtiene el reporte de consultas TransUnion
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/credito/ObtenerReporteTransUnion")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteProspeccionPreaprobado>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Reporte TransUnion",
            Description = "Consulta los reportes de TransUnion",
            OperationId = "credito.ObtenerReporteTransUnion",
            Tags = new[] { "CreditoEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteProspeccionPreaprobado>>>> HandleAsync(
            ReporteTransUnionDto request,
            CancellationToken cancellationToken = default)
        {
            return await _creditoBusinessLogic.ObtenerReporteTransUnionProspeccion(
                request.NumeroIdentificacion,
                request.FechaDesde,
                request.FechaHasta,
                request.Estado);
        }
    }
}