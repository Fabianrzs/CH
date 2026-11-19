using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Cartera.ParametersRequest.Reportes;
using GA2.Application.Dto.Cartera.Reportes;
using GA2.Application.Dto.Reportes;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ReporteGeneralMora : BaseAsyncEndpoint
        .WithRequest<ReporteGeneralMoraParametersDto>
        .WithResponse<Response<IEnumerable<CarteraReporteGeneralMoraDto>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReportePreaprobadoHipotecario
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteGeneralMora(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("api/Reportes/ReporteGeneralMora")]
        [ProducesResponseType(typeof(Response<IEnumerable<CarteraReporteGeneralMoraDto>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte general mora",
            Description = "Generar Reporte general mora",
            OperationId = "Reportes.ReporteGeneralMora",
            Tags = new[] { "ReportesEndpoint" })]
        public async override Task<ActionResult<Response<IEnumerable<CarteraReporteGeneralMoraDto>>>> HandleAsync([FromQuery]ReporteGeneralMoraParametersDto parameters, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.GenerarReporteGeneralMora(parameters);
        }
    }
}
