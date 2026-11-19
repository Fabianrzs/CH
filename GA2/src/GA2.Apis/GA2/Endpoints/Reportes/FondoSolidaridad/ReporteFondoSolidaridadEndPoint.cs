using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest.Extractos;
using GA2.Domain.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;
using System.Threading;
using System;
using GA2.Transversals.Commons;

namespace GA2.Apis.GA2.Endpoints.Reportes.FondoSolidaridad
{
    public class ReporteFondoSolidaridadEndPoint : BaseAsyncEndpoint
        .WithRequest<ReportFondoSolidaridadRequetsDto>
        .WithResponse<Response<object>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteFondoSolidaridadEndPoint(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteExtractoMensual
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteFondoSolidaridad")]
        [ProducesResponseType(typeof(Response<object>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Fondo de solidaridad",
            Description = "Generar Reporte Fondo de solidaridad",
            OperationId = "Reportes.ReporteFondoSolidaridad",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<object>>> HandleAsync( ReportFondoSolidaridadRequetsDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteFondoSolidaridad(request);
        }
    }
}
