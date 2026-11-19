
using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Haberes;
using GA2.Application.Dto.Reintegros;
using GA2.Domain.Core;
using GA2.Domain.Core.ReferenciasSvc.Haberes;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


    namespace GA2.Apis.GA2.Endpoints.Reportes
    {


    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class ReporteResumenFinanciero : BaseAsyncEndpoint
        .WithRequest<ReporteResumenFinancieroDto>
        .WithResponse<Response<FileResult>>

    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        public ReporteResumenFinanciero(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic;
        }

        /// <summary>
        /// Endpoint para iniciar el pago o desembolso de Reintegros en seven.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost("api/Reportes/ReporteResumenFinanciero")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Obtener Reporte Resumen Financiero",
            Description = "Obtener Reporte Resumen Financiero",
            OperationId = "Reportes.ReporteResumenFinanciero",
            Tags = new[] { "Reportes" })]
        public async override Task<ActionResult<Response<FileResult>>> HandleAsync(ReporteResumenFinancieroDto request, CancellationToken cancellationToken = default)
        {
            return await this._reportesBusinessLogic.ReporteResumenFinanciero(request);
        }
    }
}
