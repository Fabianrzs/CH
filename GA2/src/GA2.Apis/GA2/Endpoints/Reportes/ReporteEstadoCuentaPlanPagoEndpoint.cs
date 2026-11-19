using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteEstadoCuentaAfiliado
    /// </summary>
    [Authorize]
    public class ReporteEstadoCuentaPlanPagoEndpoint : BaseAsyncEndpoint
        .WithRequest<Guid>
        .WithResponse<Response<IEnumerable<ReporteEstadoCuentaPlanPagoDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteEstadoCuentaPlanPagoEndpoint
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteEstadoCuentaPlanPagoEndpoint(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="idCredito"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/EstadoCuentaPlanPago")]
        [ProducesResponseType(typeof(Response<IEnumerable<ReporteEstadoCuentaPlanPagoDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Estado Cuenta Plan de pago",
            Description = "Generar Reporte Estado Cuenta Plan de pago",
            OperationId = "Reportes.ReporteEstadoCuentaPlanPago",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<ReporteEstadoCuentaPlanPagoDto>>>> HandleAsync([FromQuery] Guid idCredito, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteEstadoCuentaPlanPagos(idCredito);
        }
    }
}