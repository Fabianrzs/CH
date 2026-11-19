using Ardalis.ApiEndpoints;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints
{
    /// <summary>
    /// ReporteCertificadoIngresosRetenciones
    /// </summary>
    [Authorize]
    public class CertificadoIngresosRetenciones : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// ReporteCertificadoIngresosRetenciones
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public CertificadoIngresosRetenciones(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificacion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/ReporteCertificadoIngresosRetenciones")]
        [ProducesResponseType(typeof(Response<FileResult>), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte Certificado Ingresos Retenciones",
            Description = "Generar Reporte Certificado Ingresos Retenciones",
            OperationId = "Reportes.ReporteCertificadoIngresosRetenciones",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(string identificacion, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.CertificadoIngresosRetenciones(identificacion);
        }
    }
}