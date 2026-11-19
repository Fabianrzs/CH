using Ardalis.ApiEndpoints;
using GA2.Application.Dto;
using GA2.Application.Dto.Credito;
using GA2.Application.Dto.Credito.TU;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace GA2.Apis.GA2.Endpoints.Reportes
{
    public class ReporteInformeCredito : BaseAsyncEndpoint

        .WithRequest<string>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteInformeCredito
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReporteInformeCredito(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// ReporteInformeCredito
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteInformeCredito")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte TransUnion",
            Description = "Generar Reporte TransUnion",
            OperationId = "Reportes.ReporteInformeCredito",
            Tags = new[] { "ReportesEndpoint" })]


        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(string  documento, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.ReporteInformeCredito(documento);

        }
    }

}
