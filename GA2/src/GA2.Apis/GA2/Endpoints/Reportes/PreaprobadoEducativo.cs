using Ardalis.ApiEndpoints;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using GA2.Domain.Core;
using GA2.Transversals.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GA2.Apis.GA2.Endpoints.Reportes
{
    public class PreaprobadoEducativo : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<Response<FileResult>>
    {
        private readonly IReportesBusinessLogic _reportesBusinessLogic;

        /// <summary>
        /// PreaprobadoEducativo
        /// </summary>
        /// <param name="reportesBusinessLogic"></param>
        /// <exception cref="ArgumentException"></exception>
        public PreaprobadoEducativo(IReportesBusinessLogic reportesBusinessLogic)
        {
            _reportesBusinessLogic = reportesBusinessLogic ?? throw new ArgumentException(nameof(reportesBusinessLogic));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identificacion"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("api/Reportes/PreaprobadoEducativo")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [SwaggerOperation(
            Summary = "Generar Reporte PreaprobadoEducativo",
            Description = "Generar Reporte Preaprobado educativo",
            OperationId = "Reportes.PreaprobadoEducativo",
            Tags = new[] { "ReportesEndpoint" })]
        public override async Task<ActionResult<Response<FileResult>>> HandleAsync(string identificacion, CancellationToken cancellationToken = default)
        {
            return await _reportesBusinessLogic.PreaprobadoEducativo(identificacion);
        }
    }
}
