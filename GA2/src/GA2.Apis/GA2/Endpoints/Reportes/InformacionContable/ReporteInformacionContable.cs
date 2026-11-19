using Ardalis.ApiEndpoints;
using GA2.Application.Dto.Cartera.ParametersRequest;
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

namespace GA2.Apis.GA2.Endpoints.Reportes.InformacionContable
{
    /// <summary>
    /// ReporteInformacionContable
    /// </summary>
    [Authorize]
    public class ReporteInformacionContable : BaseAsyncEndpoint
        .WithRequest<ReporteInformacionContableParametersDto>
        .WithResponse<Response<IEnumerable<InformacionContableDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteInformacionContable
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteInformacionContable(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// ReporteInformacionContable
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/InformacionContable")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Informacion Contable",
            Description = "Generar Reporte Informacion Contable",
            OperationId = "Reportes.InformacionContable",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<IEnumerable<InformacionContableDto>>>> HandleAsync([FromQuery] ReporteInformacionContableParametersDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ObtenerReporteInformacionContable(request);
        }
    }
}
