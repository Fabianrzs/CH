using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Domain.Core;
using GA2.Domain.Entities;
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
    /// ReporteFCF414
    /// </summary>
    //[Authorize]
    public class ReporteFCF453 : BaseAsyncEndpoint
        .WithRequest<ParametrosSuperfinancieraReportesDto>
        .WithResponse<Response<List<ReporteSFCF453Y454>>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteFCF453(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// SuperfinancieraReportes F414 Información
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteFCF453Y454")]
        [ProducesResponseType(typeof(Response<List<ReporteSFCF453Y454>>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Información Reporte SFC F453 y 454",
            Description = "Obtener Información Reporte SFC F453 y 454",
            OperationId = "Reportes.ReporteFCF414",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<List<ReporteSFCF453Y454>>>> HandleAsync(ParametrosSuperfinancieraReportesDto request, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteFCF453Y454(request);
        }
    }
}